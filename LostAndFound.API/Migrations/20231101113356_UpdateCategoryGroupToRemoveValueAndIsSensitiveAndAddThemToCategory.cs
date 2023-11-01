using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class UpdateCategoryGroupToRemoveValueAndIsSensitiveAndAddThemToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSensitive",
                table: "CategoryGroups");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "CategoryGroups");

            migrationBuilder.AddColumn<bool>(
                name: "IsSensitive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSensitive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Categories");

            migrationBuilder.AddColumn<bool>(
                name: "IsSensitive",
                table: "CategoryGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "CategoryGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
