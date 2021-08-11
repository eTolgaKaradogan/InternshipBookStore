using Microsoft.EntityFrameworkCore.Migrations;

namespace _03_DataAccess.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "followedUserId",
                table: "Watchlists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followedUserId",
                table: "Watchlists");
        }
    }
}
