using Microsoft.EntityFrameworkCore.Migrations;

namespace AppKurs.Data.Migrations
{
    public partial class TaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskTopic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTasks");
        }
    }
}
