using Microsoft.EntityFrameworkCore.Migrations;

namespace AppKurs.Data.Migrations
{
    public partial class ImageFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageLink",
                table: "UserTasks",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageStorageName",
                table: "UserTasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageStorageName",
                table: "UserTasks");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "UserTasks",
                newName: "ImageLink");
        }
    }
}
