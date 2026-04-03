using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcologicInnovations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ipadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubmitterIpAddress",
                table: "ContactMessages",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmitterUserAgent",
                table: "ContactMessages",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitterIpAddress",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "SubmitterUserAgent",
                table: "ContactMessages");
        }
    }
}
