using Microsoft.EntityFrameworkCore.Migrations;

namespace AppKurs.Data.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolvedTask_AspNetUsers_UserId",
                table: "SolvedTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SolvedTask_UserTasks_TaskId",
                table: "SolvedTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolvedTask",
                table: "SolvedTask");

            migrationBuilder.RenameTable(
                name: "SolvedTask",
                newName: "SolvedTasks");

            migrationBuilder.RenameIndex(
                name: "IX_SolvedTask_TaskId",
                table: "SolvedTasks",
                newName: "IX_SolvedTasks_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolvedTasks",
                table: "SolvedTasks",
                columns: new[] { "UserId", "TaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SolvedTasks_AspNetUsers_UserId",
                table: "SolvedTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolvedTasks_UserTasks_TaskId",
                table: "SolvedTasks",
                column: "TaskId",
                principalTable: "UserTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolvedTasks_AspNetUsers_UserId",
                table: "SolvedTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_SolvedTasks_UserTasks_TaskId",
                table: "SolvedTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolvedTasks",
                table: "SolvedTasks");

            migrationBuilder.RenameTable(
                name: "SolvedTasks",
                newName: "SolvedTask");

            migrationBuilder.RenameIndex(
                name: "IX_SolvedTasks_TaskId",
                table: "SolvedTask",
                newName: "IX_SolvedTask_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolvedTask",
                table: "SolvedTask",
                columns: new[] { "UserId", "TaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SolvedTask_AspNetUsers_UserId",
                table: "SolvedTask",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolvedTask_UserTasks_TaskId",
                table: "SolvedTask",
                column: "TaskId",
                principalTable: "UserTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
