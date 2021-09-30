using Microsoft.EntityFrameworkCore.Migrations;

namespace AppKurs.Data.Migrations
{
    public partial class useranswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAnswer",
                table: "SolvedTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAnswer",
                table: "SolvedTasks");
        }
    }
}
