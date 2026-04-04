using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcologicInnovations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFlagged",
                table: "ContactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                table: "ContactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFlagged",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "IsImportant",
                table: "ContactMessages");
        }
    }
}
