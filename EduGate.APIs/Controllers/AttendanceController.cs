using AutoMapper;
using EduGate.APIs.DTOs;
using EduGate.APIs.Errors;
using EduGate.Core;
using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using EduGate.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.APIs.Controllers
{
    public class AttendanceController : BaseApiController 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttendanceRepository _attendanceRepo;

        public AttendanceController(IUnitOfWork unitOfWork, IMapper mapper, IAttendanceRepository attendanceRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attendanceRepo = attendanceRepo;
        }




        // get attendance for student (app)
        [HttpGet("StudentAttendance")]
        public async Task<ActionResult<StudentAttendanceToReturnDto>> GetAttendanceForStudent(StudentAttendanceModel model)
        {

            var spec = new AttendanceSpecs(model.StudentId, model.CourseId, model.GroupId);
            var attendance = await _unitOfWork.Repository<Attendance>().GetAllWithSpecAsync(spec);


            var grouppedAttendance = attendance.GroupBy(a => a.StudentId).Select(group => new StudentAttendanceToReturnDto()
            {
                AttendanceStatus = group.Select(a => a.Attend).ToList()
            }); 


            return Ok(grouppedAttendance);


        }
        
        //(app)
        [HttpGet("history")]        
        public async Task<ActionResult> GetAttendanceHistory(int studentId)
        {
            var spec = new AttendanceSpecs(studentId);
            var attendance = await _unitOfWork.Repository<Attendance>().GetAllByIdWithSpecAsync(spec);


            var groupedAttendance = attendance
            .GroupBy(a => a.Date)
            .OrderByDescending(g => g.Key)
            .Select(group => new
            {
                Date = group.Key,
                AttendanceRecords = group.Select(a => new
                {
                    a.LectureNumber,
                    a.Course.CourseName,
                    a.Course.Code,
                    a.Group.GroupName
                }).ToList()
            });

            return Ok(groupedAttendance);
          
                
        }


        //(app)
        [HttpPost("takeAttendance")]
        public async Task<ActionResult> TakeAttendance(QrCodeData qrData)
        {
            var spec = new StudentCourseSpecs(qrData.StudentId, qrData.CourseId, qrData.GroupId);

            var studCourse = await _unitOfWork.Repository<StudentCourseGroup>().GetByIdWithSpecAsync(spec);

            if (studCourse is null) return NotFound(new ApiResponse(404, "Student not found in this course or group :|"));


            var existAttendance = await _attendanceRepo.GetAttedance(qrData.StudentId, qrData.CourseId, qrData.GroupId, qrData.LectureNumber);

            if (existAttendance == null)
            {
                return NotFound(new ApiResponse(404,"this record not found in attendacne list"));
            }

            if (existAttendance.Attend == true)
            {
                return Ok(new ApiResponse(200, "You're already there ;)"));
            }

            existAttendance.Attend = true;
            existAttendance.Date = DateTime.Now;

            _unitOfWork.Repository<Attendance>().Update(existAttendance);
            await _unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(200, "Attendance marked successfully"));

        }


        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceToReturnDto>>> GetAttendance(int courseId,int groupId)
        {

            var spec = new AttendanceSpecs(courseId, groupId);
            var attendance = await _unitOfWork.Repository<Attendance>().GetAllWithSpecAsync(spec);


            var grouppedAttendance = attendance.GroupBy(a => a.StudentId).Select(group => new AttendanceToReturnDto
            {
                StudentId = group.Key,
                StudentName = group.First().Student.Name,
                StudentAttend = group.Select(a => a.Attend).ToList()
            }); 


            return Ok(grouppedAttendance);


        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("DoctorCourse")]
        public async Task<ActionResult<AttendanceCourseForDoctorToReturnDto>> GetCoursesForDoctroAttendance(int doctorId)
        {

            var spec = new DoctorCourseSepcs(doctorId);
            var courses = await _unitOfWork.Repository<DoctorCourseGroup>().GetAllWithSpecAsync(spec);



            var grouppedcourse = courses.GroupBy(d => d.DoctorId).Select(group => new AttendanceCourseForDoctorToReturnDto
            {
                DoctorId = group.Key,
                Courses = group.Select(dc => new AttendanceCourse
                {
                    CourseId = dc.CourseId,
                    CourseName = dc.Course.CourseName,
                    CourseCode = dc.Course.Code,
                    GroupId = dc.GroupId,
                    GroupName = dc.Group.GroupName
                })



            }) ;



            return Ok(grouppedcourse);
        }

        
        [Authorize(Roles = "Doctor")]
        [HttpPost("updateAttendance")]
        public async Task<ActionResult> UpdateAttendance(UpdateAttendanceModel model)
        {
            var existAttendance = await _attendanceRepo.GetAttedance(model.StudentId, model.CourseId, model.GroupId, model.LectureNumber);

            if (existAttendance == null)
            {
                return NotFound(new ApiResponse(404));
            }

            existAttendance.Attend = model.Attend;
            existAttendance.Date = DateTime.Now;

             _unitOfWork.Repository<Attendance>().Update(existAttendance);
            await _unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(200, "attendance updated successfully"));

        }



 
        // add new attendance
        [HttpPost("addattendance")]
        public async Task<ActionResult> AddAttendanceForAllStudentCourseGroups()
        {
            try
            {
            // Retrieve all student course groups
            var allStudentCourseGroups = await _unitOfWork.Repository<StudentCourseGroup>().GetAllAsync();

            foreach (var studentCourseGroup in allStudentCourseGroups)
            {
                for (int lectureNumber = 1; lectureNumber <= 14; lectureNumber++)
                {
                    //var existingAttendance = await _unitOfWork.Repository<Attendance>()
                    //    .get(a => a.StudentCourseGroupId == studentCourseGroup.Id && a.LectureNumber == lectureNumber);

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
                     
                }
            }

                await _unitOfWork.CompleteAsync(); // Save changes to database

                return Ok("Attendance records added for all student course groups.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

         


    }
}
