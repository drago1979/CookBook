using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khaoticen.CookBook.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterReviewsTableAddNickname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "Reviews",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "Reviews");
        }
    }
}
