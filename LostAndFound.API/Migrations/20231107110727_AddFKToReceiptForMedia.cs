using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class AddFKToReceiptForMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Medias_MediaId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_MediaId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Receipts");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_ReceiptImage",
                table: "Receipts",
                column: "ReceiptImage");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Medias_ReceiptImage",
                table: "Receipts",
                column: "ReceiptImage",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Medias_ReceiptImage",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_ReceiptImage",
                table: "Receipts");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaId",
                table: "Receipts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_MediaId",
                table: "Receipts",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Medias_MediaId",
                table: "Receipts",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
