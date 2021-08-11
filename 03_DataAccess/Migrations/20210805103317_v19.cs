using Microsoft.EntityFrameworkCore.Migrations;

namespace _03_DataAccess.Migrations
{
    public partial class v19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUser_Notification_NotificationId",
                table: "NotificationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUser_Users_UserId",
                table: "NotificationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationUser",
                table: "NotificationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationUser",
                newName: "NotificationUsers");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationUser_UserId",
                table: "NotificationUsers",
                newName: "IX_NotificationUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationUsers",
                table: "NotificationUsers",
                columns: new[] { "NotificationId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUsers_Notifications_NotificationId",
                table: "NotificationUsers",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUsers_Users_UserId",
                table: "NotificationUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUsers_Notifications_NotificationId",
                table: "NotificationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUsers_Users_UserId",
                table: "NotificationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationUsers",
                table: "NotificationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "NotificationUsers",
                newName: "NotificationUser");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationUsers_UserId",
                table: "NotificationUser",
                newName: "IX_NotificationUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationUser",
                table: "NotificationUser",
                columns: new[] { "NotificationId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUser_Notification_NotificationId",
                table: "NotificationUser",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUser_Users_UserId",
                table: "NotificationUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
