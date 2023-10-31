using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class ChangeClaimnStatusInItemClaimToBoolean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CabinetId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "ClaimStatus",
                table: "ItemClaims",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CabinetId",
                table: "Items",
                column: "CabinetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Cabinets_CabinetId",
                table: "Items",
                column: "CabinetId",
                principalTable: "Cabinets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Cabinets_CabinetId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CabinetId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "CabinetId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClaimStatus",
                table: "ItemClaims",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
