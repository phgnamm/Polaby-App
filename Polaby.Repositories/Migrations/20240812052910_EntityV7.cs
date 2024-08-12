using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polaby.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationType_Notification_NotificationId",
                table: "NotificationType");

            migrationBuilder.DropIndex(
                name: "IX_NotificationType_NotificationId",
                table: "NotificationType");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "NotificationType");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "NotificationSetting");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Notification",
                newName: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationTypeId",
                table: "Notification",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationTypeId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "NotificationTypeId",
                table: "Notification",
                newName: "TypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "NotificationType",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "NotificationSetting",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationType_NotificationId",
                table: "NotificationType",
                column: "NotificationId",
                unique: true,
                filter: "[NotificationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationType_Notification_NotificationId",
                table: "NotificationType",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id");
        }
    }
}
