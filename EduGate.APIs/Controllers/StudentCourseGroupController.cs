using AutoMapper;
using EduGate.APIs.DTOs;
using EduGate.APIs.Errors;
using EduGate.Core;
using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using EduGate.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduGate.APIs.Controllers
{
    public class StudentCourseGroupController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentCourseGroupController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCourseToStudent(StudentCourseGroupModel model)
        {
            var student  = await _unitOfWork.Repository<Student>().GetByIdAsync(model.StudentId);
            if (student is null)
                return NotFound(new ApiResponse(404, "Student does not exist"));

             var course  = await _unitOfWork.Repository<Course>().GetByIdAsync(model.CourseId);
            if (course is null)
                return NotFound(new ApiResponse(404, "Course does not exist"));

             var group  = await _unitOfWork.Repository<Group>().GetByIdAsync(model.GroupId);
            if (group is null)
                return NotFound(new ApiResponse(404, "Group does not exist"));

            var spec = new StudentCourseSpecs(model.StudentId, model.CourseId);

            var studentCourse = await _unitOfWork.Repository<StudentCourseGroup>().GetByIdWithSpecAsync(spec);

            if (studentCourse is not null)
            {
                return BadRequest(new ApiResponse(400, "student aleady exist in this course"));
            }

            var studentCourseGroup = new StudentCourseGroup()
            {
                StudentId = model.StudentId,
                CourseId = model.CourseId,
                GroupId = model.GroupId
            };

            await _unitOfWork.Repository<StudentCourseGroup>().AddAsync(studentCourseGroup);
            var count = await _unitOfWork.CompleteAsync();

            if (count > 0)
            {
                for (int lectureNumber = 1; lectureNumber <= 14; lectureNumber++)
                {
                    var attendance = new Attendance
                    {
                        StudentId = studentCourseGroup.StudentId,
                        CourseId = studentCourseGroup.CourseId,
                        GroupId = studentCourseGroup.GroupId,
                        LectureNumber = lectureNumber,
                        Attend = false,
                        Date = DateTime.Now,
                    };

                    await _unitOfWork.Repository<Attendance>().AddAsync(attendance);
                    await _unitOfWork.CompleteAsync();
                }

                
            }
            return Ok(new ApiResponse(200, "Student added to course group successfully"));
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllStudentCourseGroup()
        {
            var spec = new StudentCourseSpecs();

            var students = await _unitOfWork.Repository<StudentCourseGroup>().GetAllWithSpecAsync(spec);

            //var studentMapped = _mapper.Map<IEnumerable<StudentCourseGroup>,IEnumerable<studentCourseToReturnDto>>(students);

            var formattedOutput = students
            .GroupBy(sc => new { sc.StudentId, sc.Student.Name }) // Group by StudentId and StudentName
            .Select(group => new
            {
                studentid = group.Key.StudentId,
                studentname = group.Key.Name,
                courses = group.Select(sc => new
                {
                    coursename = sc.Course.CourseName,
                    courseId = sc.CourseId,
                    groupid = sc.GroupId,
                    group = sc.Group.GroupName
                }).ToList()
            });
            return Ok(formattedOutput);
        }



        // (app)Student 
        [HttpGet("AppStudentCourse")]
        public async Task<ActionResult<IEnumerable<StudentCourseGroup>>> GetCoursesForStudent(int studentId)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(studentId);
            if (student is null) return NotFound(new ApiResponse(404));

            var spec = new StudentCourseSpecs(studentId);
            var studentCourse = await _unitOfWork.Repository<StudentCourseGroup>().GetAllByIdWithSpecAsync(spec);
            if (studentCourse is null) return NotFound(new ApiResponse(404));

            var mapped = _mapper.Map <IEnumerable<StudentCourseGroup>, IEnumerable<SpecificCourseGroupForStudentToReturnDto>>(studentCourse);

            return Ok(mapped);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteCourseFromStudent")]
        public async Task<ActionResult> DeleteStudentFromCourse(StudentCourseGroupModel model)
        {
            var spec = new StudentCourseSpecs(model.StudentId, model.CourseId, model.GroupId);

            var studentCourse = await _unitOfWork.Repository<StudentCourseGroup>().GetByIdWithSpecAsync(spec);

            if (studentCourse is null)
            {
                return NotFound(new ApiResponse(404));
            }


            try
            {
                _unitOfWork.Repository<StudentCourseGroup>().Delete(studentCourse);
            }
            catch 
            {

                return BadRequest(400);
            }

            var attendSpec = new AttendanceSpecs(model.StudentId, model.CourseId, model.GroupId);
            var attendance = await _unitOfWork.Repository<Attendance>().GetAllByIdWithSpecAsync(attendSpec);
            if (attendance is not null) 
            {
                 _unitOfWork.Repository<Attendance>().DeleteAll(attendance);
                await _unitOfWork.CompleteAsync();
            }

            return Ok(new ApiResponse(200, "Course deleted from student successfully"));
        }

    }


}
