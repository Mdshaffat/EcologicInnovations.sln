using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Builds common breadcrumb trails used across public pages.
/// Keeping this in a helper avoids repeating the same breadcrumb composition in controllers.
/// </summary>
public static class BreadcrumbBuilder
{
    public static List<BreadcrumbItemViewModel> CreateForHome()
    {
        return
        [
            new BreadcrumbItemViewModel
            {
                Title = "Home",
                Url = "/",
                IsActive = true
            }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForAbout()
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "About Us", Url = null, IsActive = true }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForPolicy()
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "Policy", Url = null, IsActive = true }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForProducts()
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "Products", Url = null, IsActive = true }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForProductDetails(string productTitle)
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "Products", Url = "/products", IsActive = false },
            new BreadcrumbItemViewModel { Title = productTitle, Url = null, IsActive = true }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForBlog()
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "Articles", Url = null, IsActive = true }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForBlogDetails(string blogTitle)
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "Articles", Url = "/blog", IsActive = false },
            new BreadcrumbItemViewModel { Title = blogTitle, Url = null, IsActive = true }
        ];
    }

    public static List<BreadcrumbItemViewModel> CreateForContact()
    {
        return
        [
            new BreadcrumbItemViewModel { Title = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemViewModel { Title = "Contact", Url = null, IsActive = true }
        ];
    }
}
