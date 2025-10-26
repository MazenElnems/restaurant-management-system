using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameDishesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishs_Categories_CategoryId",
                table: "Dishs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dishs",
                table: "Dishs");

            migrationBuilder.RenameTable(
                name: "Dishs",
                newName: "Dishes");

            migrationBuilder.RenameIndex(
                name: "IX_Dishs_CategoryId",
                table: "Dishes",
                newName: "IX_Dishes_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dishes",
                table: "Dishes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Categories_CategoryId",
                table: "Dishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dishes",
                table: "Dishes");

            migrationBuilder.RenameTable(
                name: "Dishes",
                newName: "Dishs");

            migrationBuilder.RenameIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishs",
                newName: "IX_Dishs_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dishs",
                table: "Dishs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishs_Categories_CategoryId",
                table: "Dishs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
