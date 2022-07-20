using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mednik.Migrations
{
    public partial class new_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgURL",
                table: "AspNetUsers",
                newName: "ImgUrl");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "AspNetUsers",
                newName: "ImgURL");
        }
    }
}
