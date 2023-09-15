using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class AddItemFlagAndItemBookmarkTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemBookMarks",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBookMarks", x => new { x.ItemId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ItemBookMarks_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemBookMarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemFlags",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemFlagReason = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemFlags", x => new { x.ItemId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ItemFlags_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemFlags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemBookMarks_UserId",
                table: "ItemBookMarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemFlags_UserId",
                table: "ItemFlags",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemBookMarks");

            migrationBuilder.DropTable(
                name: "ItemFlags");
        }
    }
}
