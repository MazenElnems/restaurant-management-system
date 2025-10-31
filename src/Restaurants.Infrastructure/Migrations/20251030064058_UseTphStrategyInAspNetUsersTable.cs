using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseTphStrategyInAspNetUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");


            // intialize existing users with their respective types
            migrationBuilder.Sql(
                @"
                    UPDATE AspNetUsers
                    SET UserType = 'Admin' WHERE UserName = 'mazenelnems22got@gmail.com';    
                ");

            migrationBuilder.Sql(
                @"
                    UPDATE AspNetUsers
                    SET UserType = 'Owner' WHERE UserName = 'owner@gmail.com';    
                ");

            migrationBuilder.Sql(
                @"
                    UPDATE AspNetUsers
                    SET UserType = 'Owner' WHERE UserName = 'mohamed@gmail.com';    
                ");

            migrationBuilder.Sql(
                @"
                    UPDATE AspNetUsers
                    SET UserType = 'Staff' WHERE UserName = 'staff@gmail.com';    
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");
        }
    }
}
