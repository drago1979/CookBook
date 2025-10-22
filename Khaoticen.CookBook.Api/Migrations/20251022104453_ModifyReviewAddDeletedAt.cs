using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khaoticen.CookBook.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModifyReviewAddDeletedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Reviews",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Reviews");
        }
    }
}
