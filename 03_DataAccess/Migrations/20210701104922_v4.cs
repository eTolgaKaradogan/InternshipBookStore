using Microsoft.EntityFrameworkCore.Migrations;

namespace _03_DataAccess.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "IsEnable",
                table: "Books",
                newName: "IsEnabled");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "Books",
                newName: "IsEnable");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "UnitPrice",
                table: "Books",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
