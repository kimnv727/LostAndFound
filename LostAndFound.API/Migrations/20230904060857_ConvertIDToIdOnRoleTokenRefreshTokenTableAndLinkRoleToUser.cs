using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LostAndFound.API.Migrations
{
    public partial class ConvertIDToIdOnRoleTokenRefreshTokenTableAndLinkRoleToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Tokens_TokenID",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TokenID",
                table: "RefreshTokens",
                newName: "TokenId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "RefreshTokens",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_TokenID",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_TokenId");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Tokens_TokenId",
                table: "RefreshTokens",
                column: "TokenId",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Tokens_TokenId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tokens",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "TokenId",
                table: "RefreshTokens",
                newName: "TokenID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RefreshTokens",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_TokenId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_TokenID");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Tokens_TokenID",
                table: "RefreshTokens",
                column: "TokenID",
                principalTable: "Tokens",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
