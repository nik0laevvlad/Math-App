using Microsoft.EntityFrameworkCore.Migrations;

namespace AppKurs.Data.Migrations
{
    public partial class fix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "SolvedTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SolvedTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
