using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Repositroy.Identity.Migrations
{
    public partial class EditDoctor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Doctors");
        }
    }
}
