using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcologicInnovations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSiteSettingsPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE SiteSettings SET Phone = '+8801517831132' WHERE Phone = '+880-0000-000000'");

            migrationBuilder.Sql(
                "UPDATE SiteSettings SET Tagline = 'Software development, smart systems, training & development, and impact-driven technology.' WHERE Tagline LIKE '%sustainability IoT%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE SiteSettings SET Phone = '+880-0000-000000' WHERE Phone = '+8801517831132'");

            migrationBuilder.Sql(
                "UPDATE SiteSettings SET Tagline = 'Software, sustainability IoT devices, energy equipment, and eco-focused digital solutions.' WHERE Tagline LIKE '%smart systems%'");
        }
    }
}
