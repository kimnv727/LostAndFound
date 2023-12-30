using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class ChangePostLocationAndCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostCategory",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostLocation",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "CategoryPost",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    PostsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPost", x => new { x.CategoriesId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CategoryPost_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationPost",
                columns: table => new
                {
                    LocationsId = table.Column<int>(type: "int", nullable: false),
                    PostsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationPost", x => new { x.LocationsId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_LocationPost_Locations_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPost_PostsId",
                table: "CategoryPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationPost_PostsId",
                table: "LocationPost",
                column: "PostsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryPost");

            migrationBuilder.DropTable(
                name: "LocationPost");

            migrationBuilder.AddColumn<string>(
                name: "PostCategory",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostLocation",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
