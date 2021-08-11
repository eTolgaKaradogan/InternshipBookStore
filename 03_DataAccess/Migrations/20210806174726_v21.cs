using Microsoft.EntityFrameworkCore.Migrations;

namespace _03_DataAccess.Migrations
{
    public partial class v21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "NotificationUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "NotificationUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
