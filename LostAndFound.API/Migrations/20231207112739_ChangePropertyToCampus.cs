using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class ChangePropertyToCampus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Campuses_PropertyId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Locations",
                newName: "CampusId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_PropertyId",
                table: "Locations",
                newName: "IX_Locations_CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Campuses_CampusId",
                table: "Locations",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Campuses_CampusId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "CampusId",
                table: "Locations",
                newName: "PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_CampusId",
                table: "Locations",
                newName: "IX_Locations_PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Campuses_PropertyId",
                table: "Locations",
                column: "PropertyId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
