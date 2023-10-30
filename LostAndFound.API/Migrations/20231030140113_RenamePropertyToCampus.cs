using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class RenamePropertyToCampus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Properties_PropertyId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserViolationReports_ViolationReports_ReportId",
                table: "UserViolationReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ViolationReports",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Campuses");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Reports");

            migrationBuilder.RenameTable(
                name: "Campuses",
                newName: "Campuses");

            migrationBuilder.RenameColumn(
                name: "PropertyName",
                table: "Campuses",
                newName: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campuses",
                table: "Campuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Campuses_PropertyId",
                table: "Locations",
                column: "PropertyId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserViolationReports_Reports_ReportId",
                table: "UserViolationReports",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Campuses_PropertyId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserViolationReports_Reports_ReportId",
                table: "UserViolationReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campuses",
                table: "Campuses");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Reports");

            migrationBuilder.RenameTable(
                name: "Campuses",
                newName: "Campuses");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Campuses",
                newName: "PropertyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ViolationReports",
                table: "Reports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Campuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Properties_PropertyId",
                table: "Locations",
                column: "PropertyId",
                principalTable: "Campuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserViolationReports_Reports_ReportId",
                table: "UserViolationReports",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
