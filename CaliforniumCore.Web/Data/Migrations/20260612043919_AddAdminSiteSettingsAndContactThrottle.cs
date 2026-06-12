using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaliforniumCore.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminSiteSettingsAndContactThrottle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleMapEmbedUrl",
                table: "SiteSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue1Description",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue1IconCssClass",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue1Title",
                table: "SiteSettings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue2Description",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue2IconCssClass",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue2Title",
                table: "SiteSettings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue3Description",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue3IconCssClass",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue3Title",
                table: "SiteSettings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue4Description",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue4IconCssClass",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValue4Title",
                table: "SiteSettings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValueIntro",
                table: "SiteSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValueKicker",
                table: "SiteSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeValueTitle",
                table: "SiteSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmitterDeviceId",
                table: "ContactMessages",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_SubmitterDeviceId_CreatedAt",
                table: "ContactMessages",
                columns: new[] { "SubmitterDeviceId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactMessages_SubmitterDeviceId_CreatedAt",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "GoogleMapEmbedUrl",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue1Description",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue1IconCssClass",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue1Title",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue2Description",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue2IconCssClass",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue2Title",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue3Description",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue3IconCssClass",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue3Title",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue4Description",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue4IconCssClass",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValue4Title",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValueIntro",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValueKicker",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "HomeValueTitle",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "SubmitterDeviceId",
                table: "ContactMessages");
        }
    }
}
