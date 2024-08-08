using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSetting_AspNetUsers_UserId",
                table: "NotificationSetting");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CommunityPost");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NotificationSetting",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationSetting_UserId",
                table: "NotificationSetting",
                newName: "IX_NotificationSetting_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSetting_AspNetUsers_AccountId",
                table: "NotificationSetting",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSetting_AspNetUsers_AccountId",
                table: "NotificationSetting");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "NotificationSetting",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationSetting_AccountId",
                table: "NotificationSetting",
                newName: "IX_NotificationSetting_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CommunityPost",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSetting_AspNetUsers_UserId",
                table: "NotificationSetting",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
