using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcologicInnovations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PublicUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    AltText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MediaGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UploadedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SitePages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BannerImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShortIntro = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SitePages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tagline = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FaviconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SalesEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FooterHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    YouTubeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetaTitleDefault = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MetaDescriptionDefault = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    BlogCategoryId = table.Column<int>(type: "int", nullable: true),
                    FeatureImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Excerpt = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false),
                    HtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowContactForm = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ContactFormTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MainImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HtmlDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactFormEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ContactFormTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShowInProductMenu = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MenuSortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ListSortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OgImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    BlogPostId = table.Column<int>(type: "int", nullable: true),
                    SourceTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "New"),
                    AdminNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactMessages_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ContactMessages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_CreatedAt",
                table: "BlogCategories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_IsActive_SortOrder_Name",
                table: "BlogCategories",
                columns: new[] { "IsActive", "SortOrder", "Name" });

            migrationBuilder.CreateIndex(
                name: "UX_BlogCategories_Slug",
                table: "BlogCategories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Category_Publish_PublishedAt_CreatedAt",
                table: "BlogPosts",
                columns: new[] { "BlogCategoryId", "IsPublished", "PublishedAt", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CreatedAt",
                table: "BlogPosts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Featured_Publish_PublishedAt",
                table: "BlogPosts",
                columns: new[] { "IsFeatured", "IsPublished", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Publish_PublishedAt_CreatedAt",
                table: "BlogPosts",
                columns: new[] { "IsPublished", "PublishedAt", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "UX_BlogPosts_Slug",
                table: "BlogPosts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_BlogPostId",
                table: "ContactMessages",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_CreatedAt",
                table: "ContactMessages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_ProductId",
                table: "ContactMessages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_SourceType_CreatedAt",
                table: "ContactMessages",
                columns: new[] { "SourceType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_Status_CreatedAt",
                table: "ContactMessages",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CreatedAt",
                table: "MediaFiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_IsActive_UploadedAt",
                table: "MediaFiles",
                columns: new[] { "IsActive", "UploadedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MediaGroup_UploadedAt",
                table: "MediaFiles",
                columns: new[] { "MediaGroup", "UploadedAt" });

            migrationBuilder.CreateIndex(
                name: "UX_MediaFiles_FilePath",
                table: "MediaFiles",
                column: "FilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_MediaFiles_PublicUrl",
                table: "MediaFiles",
                column: "PublicUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CreatedAt",
                table: "ProductCategories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_IsActive_SortOrder_Name",
                table: "ProductCategories",
                columns: new[] { "IsActive", "SortOrder", "Name" });

            migrationBuilder.CreateIndex(
                name: "UX_ProductCategories_Slug",
                table: "ProductCategories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category_Publish_Active_ListSort_CreatedAt",
                table: "Products",
                columns: new[] { "ProductCategoryId", "IsPublished", "IsActive", "ListSortOrder", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedAt",
                table: "Products",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Featured_Publish_Active_CreatedAt",
                table: "Products",
                columns: new[] { "IsFeatured", "IsPublished", "IsActive", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MenuDropdown",
                table: "Products",
                columns: new[] { "ShowInProductMenu", "IsPublished", "IsActive", "MenuSortOrder", "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Publish_Active_CreatedAt",
                table: "Products",
                columns: new[] { "IsPublished", "IsActive", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "UX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SitePages_IsPublished_SortOrder_CreatedAt",
                table: "SitePages",
                columns: new[] { "IsPublished", "SortOrder", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "UX_SitePages_PageKey",
                table: "SitePages",
                column: "PageKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_SitePages_Slug",
                table: "SitePages",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteSettings_CreatedAt",
                table: "SiteSettings",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "SitePages");

            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
