using EcologicInnovations.Web.Data;
using EcologicInnovations.Web.Helpers;
using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using EcologicInnovations.Web.Services.Interfaces;
using EcologicInnovations.Web.ViewModels.Contact;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcologicInnovations.Web.Controllers;

/// <summary>
/// Public Contact page controller.
/// This controller supports both general contact submissions and optional
/// source-aware submissions when the Contact page is opened for a Product or Blog context.
/// </summary>
[Route("contact")]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISiteSettingsService _siteSettingsService;
    private readonly ICanonicalUrlService _canonicalUrlService;

    public ContactController(
        ApplicationDbContext dbContext,
        ISiteSettingsService siteSettingsService,
        ICanonicalUrlService canonicalUrlService)
    {
        _dbContext = dbContext;
        _siteSettingsService = siteSettingsService;
        _canonicalUrlService = canonicalUrlService;
    }

    /// <summary>
    /// Public contact page.
    /// Optional querystring source context is resolved here.
    /// Examples:
    /// /contact
    /// /contact?productId=12
    /// /contact?blogPostId=9
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> Index(
        int? productId,
        int? blogPostId,
        CancellationToken cancellationToken = default)
    {
        var model = await BuildContactPageViewModelAsync(
            productId,
            blogPostId,
            formOverride: null,
            cancellationToken: cancellationToken);

        ViewData.SetSeoMeta(model.Seo);
        return View(model);
    }

    /// <summary>
    /// Handles the public contact form submission.
    /// The message is stored in ContactMessages with General, Product, or Blog source tracking.
    /// </summary>
    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        ContactFormInputModel input,
        CancellationToken cancellationToken = default)
    {
        await ResolvePostedSourceAsync(input, cancellationToken);

        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildContactPageViewModelAsync(
                input.ProductId,
                input.BlogPostId,
                formOverride: input,
                cancellationToken: cancellationToken);

            ViewData.SetSeoMeta(invalidModel.Seo);
            return View(invalidModel);
        }

        var entity = new ContactMessage
        {
            Name = input.Name.Trim(),
            Email = input.Email.Trim(),
            Phone = input.Phone.Trim(),
            Company = string.IsNullOrWhiteSpace(input.Company) ? null : input.Company.Trim(),
            Subject = string.IsNullOrWhiteSpace(input.Subject) ? null : input.Subject.Trim(),
            Message = input.Message.Trim(),
            SourceType = input.SourceType,
            ProductId = input.SourceType == ContactSourceType.Product ? input.ProductId : null,
            BlogPostId = input.SourceType == ContactSourceType.Blog ? input.BlogPostId : null,
            SourceTitle = string.IsNullOrWhiteSpace(input.SourceTitle) ? null : input.SourceTitle.Trim(),
            PageUrl = string.IsNullOrWhiteSpace(input.PageUrl) ? "/contact" : input.PageUrl.Trim(),
            Status = ContactMessageStatus.New,
            AdminNote = null
        };

        _dbContext.ContactMessages.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var successModel = await BuildContactPageViewModelAsync(
            input.ProductId,
            input.BlogPostId,
            formOverride: new ContactFormInputModel
            {
                SourceType = input.SourceType,
                ProductId = input.ProductId,
                BlogPostId = input.BlogPostId,
                SourceTitle = input.SourceTitle,
                PageUrl = input.PageUrl,
                RegardingProductTitle = input.RegardingProductTitle
            },
            cancellationToken: cancellationToken);

        successModel.SubmittedSuccessfully = true;
        successModel.SuccessMessage = "Thank you. Your message has been sent successfully. Our team will contact you soon.";

        ModelState.Clear();
        ViewData.SetSeoMeta(successModel.Seo);
        return View(successModel);
    }

    private async Task<ContactPageViewModel> BuildContactPageViewModelAsync(
        int? productId,
        int? blogPostId,
        ContactFormInputModel? formOverride,
        CancellationToken cancellationToken)
    {
        var siteSettings = await _siteSettingsService.GetPrimaryOrDefaultAsync(cancellationToken);
        var requestPath = Request.Path.HasValue ? Request.Path.Value! : "/contact";
        var canonicalUrl = _canonicalUrlService.BuildAbsoluteCurrentUrl(HttpContext.Request);

        var model = new ContactPageViewModel
        {
            PageTitle = "Contact Us",
            IntroText = "Tell us what you need. We support software, sustainability IoT devices, energy equipment, and business-focused eco solutions.",
            SupportEmail = siteSettings.SupportEmail,
            SalesEmail = siteSettings.SalesEmail,
            Phone = siteSettings.Phone,
            Address = siteSettings.Address,
            Breadcrumbs = new List<BreadcrumbItemViewModel>
            {
                new() { Title = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new() { Title = "Contact", Url = null, IsActive = true }
            },
            Seo = new SeoMetaViewModel
            {
                Title = "Contact Us",
                Description = "Contact Ecologic Innovations for software, sustainability IoT devices, energy equipment, and eco-technology business solutions.",
                CanonicalUrl = canonicalUrl,
                OgTitle = "Contact Us",
                OgDescription = "Contact Ecologic Innovations for practical business and sustainability solutions.",
                Robots = "index,follow"
            }
        };

        model.Form = formOverride ?? new ContactFormInputModel
        {
            SourceType = ContactSourceType.General,
            PageUrl = requestPath
        };

        if (productId.HasValue && productId.Value > 0)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Where(x => x.Id == productId.Value && x.IsActive && x.IsPublished)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Slug
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product is not null)
            {
                model.SourceType = ContactSourceType.Product;
                model.SourceHeading = "Regarding Product";
                model.SourceDisplayTitle = product.Title;

                model.Form.SourceType = ContactSourceType.Product;
                model.Form.ProductId = product.Id;
                model.Form.BlogPostId = null;
                model.Form.SourceTitle = product.Title;
                model.Form.PageUrl = $"/products/{product.Slug}";
                model.Form.RegardingProductTitle = product.Title;

                model.Seo.Title = $"Contact Us About {product.Title}";
                model.Seo.Description = $"Send an inquiry about {product.Title} to Ecologic Innovations.";
                model.Seo.OgTitle = model.Seo.Title;
                model.Seo.OgDescription = model.Seo.Description;
                return model;
            }
        }

        if (blogPostId.HasValue && blogPostId.Value > 0)
        {
            var blog = await _dbContext.BlogPosts
                .AsNoTracking()
                .Where(x => x.Id == blogPostId.Value && x.IsPublished)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Slug
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (blog is not null)
            {
                model.SourceType = ContactSourceType.Blog;
                model.SourceHeading = "Regarding Article";
                model.SourceDisplayTitle = blog.Title;

                model.Form.SourceType = ContactSourceType.Blog;
                model.Form.ProductId = null;
                model.Form.BlogPostId = blog.Id;
                model.Form.SourceTitle = blog.Title;
                model.Form.PageUrl = $"/blog/{blog.Slug}";
                model.Form.RegardingProductTitle = null;

                model.Seo.Title = $"Contact Us About {blog.Title}";
                model.Seo.Description = $"Send an inquiry related to the blog article {blog.Title}.";
                model.Seo.OgTitle = model.Seo.Title;
                model.Seo.OgDescription = model.Seo.Description;
            }
        }

        return model;
    }

    /// <summary>
    /// Re-resolves posted source information from the database so message source tracking
    /// stays trustworthy and does not depend on user-manipulated hidden fields.
    /// </summary>
    private async Task ResolvePostedSourceAsync(
        ContactFormInputModel input,
        CancellationToken cancellationToken)
    {
        input.SourceType = ContactSourceType.General;
        input.SourceTitle = null;
        input.PageUrl = "/contact";
        input.RegardingProductTitle = null;

        if (input.ProductId.HasValue && input.ProductId.Value > 0)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Where(x => x.Id == input.ProductId.Value && x.IsActive && x.IsPublished)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Slug
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product is not null)
            {
                input.SourceType = ContactSourceType.Product;
                input.ProductId = product.Id;
                input.BlogPostId = null;
                input.SourceTitle = product.Title;
                input.PageUrl = $"/products/{product.Slug}";
                input.RegardingProductTitle = product.Title;
                return;
            }
        }

        if (input.BlogPostId.HasValue && input.BlogPostId.Value > 0)
        {
            var blog = await _dbContext.BlogPosts
                .AsNoTracking()
                .Where(x => x.Id == input.BlogPostId.Value && x.IsPublished)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Slug
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (blog is not null)
            {
                input.SourceType = ContactSourceType.Blog;
                input.ProductId = null;
                input.BlogPostId = blog.Id;
                input.SourceTitle = blog.Title;
                input.PageUrl = $"/blog/{blog.Slug}";
                input.RegardingProductTitle = null;
                return;
            }
        }

        input.ProductId = null;
        input.BlogPostId = null;
    }
}
