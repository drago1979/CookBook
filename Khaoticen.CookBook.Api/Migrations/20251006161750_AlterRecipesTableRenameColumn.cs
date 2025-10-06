using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khaoticen.CookBook.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterRecipesTableRenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Recipes",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Recipes",
                newName: "Url");
        }
    }
}
