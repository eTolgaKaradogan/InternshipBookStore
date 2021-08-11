using Microsoft.EntityFrameworkCore.Migrations;

namespace _03_DataAccess.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlists_Users_UserId",
                table: "Watchlists");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlists_Users_UserId",
                table: "Watchlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlists_Users_UserId",
                table: "Watchlists");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlists_Users_UserId",
                table: "Watchlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
