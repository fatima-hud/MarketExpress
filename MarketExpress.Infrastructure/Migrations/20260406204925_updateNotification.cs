using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_CreatedBy",
                schema: "dbo",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_Notification_NotificationId",
                schema: "dbo",
                table: "UserNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotification_Users_UserId",
                schema: "dbo",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotification",
                schema: "dbo",
                table: "UserNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                schema: "dbo",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "UserNotification",
                schema: "dbo",
                newName: "UserNotifications",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Notification",
                schema: "dbo",
                newName: "Notifications",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_UserId",
                schema: "dbo",
                table: "UserNotifications",
                newName: "IX_UserNotifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotification_NotificationId",
                schema: "dbo",
                table: "UserNotifications",
                newName: "IX_UserNotifications_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_CreatedBy",
                schema: "dbo",
                table: "Notifications",
                newName: "IX_Notifications_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotifications",
                schema: "dbo",
                table: "UserNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                schema: "dbo",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_CreatedBy",
                schema: "dbo",
                table: "Notifications",
                column: "CreatedBy",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Notifications_NotificationId",
                schema: "dbo",
                table: "UserNotifications",
                column: "NotificationId",
                principalSchema: "dbo",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                schema: "dbo",
                table: "UserNotifications",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_CreatedBy",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Notifications_NotificationId",
                schema: "dbo",
                table: "UserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                schema: "dbo",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotifications",
                schema: "dbo",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                schema: "dbo",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "UserNotifications",
                schema: "dbo",
                newName: "UserNotification",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Notifications",
                schema: "dbo",
                newName: "Notification",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_UserId",
                schema: "dbo",
                table: "UserNotification",
                newName: "IX_UserNotification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_NotificationId",
                schema: "dbo",
                table: "UserNotification",
                newName: "IX_UserNotification_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CreatedBy",
                schema: "dbo",
                table: "Notification",
                newName: "IX_Notification_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotification",
                schema: "dbo",
                table: "UserNotification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                schema: "dbo",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_CreatedBy",
                schema: "dbo",
                table: "Notification",
                column: "CreatedBy",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_Notification_NotificationId",
                schema: "dbo",
                table: "UserNotification",
                column: "NotificationId",
                principalSchema: "dbo",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotification_Users_UserId",
                schema: "dbo",
                table: "UserNotification",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
