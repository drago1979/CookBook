using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khaoticen.CookBook.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterReviewsTableAddRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipeId",
                table: "Reviews",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RecipeId",
                table: "Reviews",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Recipes_RecipeId",
                table: "Reviews",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Recipes_RecipeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RecipeId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "Reviews");
        }
    }
}
