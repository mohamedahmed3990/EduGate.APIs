using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduGate.Repositroy.Identity.Migrations
{
    public partial class UpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_CourseGroup_CourseGroupId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorCourseGroup_CourseGroup_CourseGroupId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseGroup_CourseGroup_CourseGroupId",
                table: "StudentCourseGroup");

            migrationBuilder.DropTable(
                name: "CourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseGroupId",
                table: "StudentCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_DoctorCourseGroup_CourseGroupId_DoctorId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_StudentId_CourseGroupId_LectureNumber",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "CourseGroupId",
                table: "StudentCourseGroup",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourseGroup_CourseGroupId",
                table: "StudentCourseGroup",
                newName: "IX_StudentCourseGroup_GroupId");

            migrationBuilder.RenameColumn(
                name: "CourseGroupId",
                table: "DoctorCourseGroup",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "CourseGroupId",
                table: "Attendance",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_CourseGroupId",
                table: "Attendance",
                newName: "IX_Attendance_GroupId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "StudentCourseGroup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "DoctorCourseGroup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseGroup_CourseId",
                table: "StudentCourseGroup",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseId_GroupId",
                table: "StudentCourseGroup",
                columns: new[] { "StudentId", "CourseId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCourseGroup_CourseId_GroupId_DoctorId",
                table: "DoctorCourseGroup",
                columns: new[] { "CourseId", "GroupId", "DoctorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCourseGroup_GroupId",
                table: "DoctorCourseGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_CourseId",
                table: "Attendance",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId_CourseId_GroupId_LectureNumber",
                table: "Attendance",
                columns: new[] { "StudentId", "CourseId", "GroupId", "LectureNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Course_CourseId",
                table: "Attendance",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Group_GroupId",
                table: "Attendance",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorCourseGroup_Course_CourseId",
                table: "DoctorCourseGroup",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorCourseGroup_Group_GroupId",
                table: "DoctorCourseGroup",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseGroup_Course_CourseId",
                table: "StudentCourseGroup",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseGroup_Group_GroupId",
                table: "StudentCourseGroup",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Course_CourseId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Group_GroupId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorCourseGroup_Course_CourseId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorCourseGroup_Group_GroupId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseGroup_Course_CourseId",
                table: "StudentCourseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseGroup_Group_GroupId",
                table: "StudentCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourseGroup_CourseId",
                table: "StudentCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseId_GroupId",
                table: "StudentCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_DoctorCourseGroup_CourseId_GroupId_DoctorId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_DoctorCourseGroup_GroupId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_CourseId",
                table: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendance_StudentId_CourseId_GroupId_LectureNumber",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "StudentCourseGroup");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "DoctorCourseGroup");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "StudentCourseGroup",
                newName: "CourseGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourseGroup_GroupId",
                table: "StudentCourseGroup",
                newName: "IX_StudentCourseGroup_CourseGroupId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "DoctorCourseGroup",
                newName: "CourseGroupId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Attendance",
                newName: "CourseGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendance_GroupId",
                table: "Attendance",
                newName: "IX_Attendance_CourseGroupId");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseGroup_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseGroup_StudentId_CourseGroupId",
                table: "StudentCourseGroup",
                columns: new[] { "StudentId", "CourseGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorCourseGroup_CourseGroupId_DoctorId",
                table: "DoctorCourseGroup",
                columns: new[] { "CourseGroupId", "DoctorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_StudentId_CourseGroupId_LectureNumber",
                table: "Attendance",
                columns: new[] { "StudentId", "CourseGroupId", "LectureNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroup_CourseId",
                table: "CourseGroup",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseGroup_GroupId_CourseId",
                table: "CourseGroup",
                columns: new[] { "GroupId", "CourseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_CourseGroup_CourseGroupId",
                table: "Attendance",
                column: "CourseGroupId",
                principalTable: "CourseGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorCourseGroup_CourseGroup_CourseGroupId",
                table: "DoctorCourseGroup",
                column: "CourseGroupId",
                principalTable: "CourseGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseGroup_CourseGroup_CourseGroupId",
                table: "StudentCourseGroup",
                column: "CourseGroupId",
                principalTable: "CourseGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
