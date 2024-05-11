using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Repositroy.Identity.Migrations
{
    public partial class EditStudentCourseGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseId_GroupId",
                table: "StudentCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_StudentId_CourseId_GroupId_LectureNumber",
                table: "Attendance");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseId",
                table: "StudentCourseGroup",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseId",
                table: "StudentCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_StudentId",
                table: "Attendance");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseId_GroupId",
                table: "StudentCourseGroup",
                columns: new[] { "StudentId", "CourseId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId_CourseId_GroupId_LectureNumber",
                table: "Attendance",
                columns: new[] { "StudentId", "CourseId", "GroupId", "LectureNumber" },
                unique: true);
        }
    }
}
