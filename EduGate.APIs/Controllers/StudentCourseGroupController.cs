﻿using AutoMapper;
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
                return NotFound(new ApiResponse(404));

             var course  = await _unitOfWork.Repository<Course>().GetByIdAsync(model.CourseId);
            if (course is null)
                return NotFound(new ApiResponse(404));

             var group  = await _unitOfWork.Repository<Group>().GetByIdAsync(model.GroupId);
            if (group is null)
                return NotFound(new ApiResponse(404));

            var spec = new StudentCourseSpecs(model.StudentId, model.CourseId, model.GroupId);

            var studentCourse = await _unitOfWork.Repository<StudentCourseGroup>().GetByIdWithSpecAsync(spec);

            if (studentCourse is not null)
            {
                return BadRequest(new ApiResponse(400, "student aleady exist!!"));
            }

            var studentCourseGroup = new StudentCourseGroup()
            {
                StudentId = model.StudentId,
                CourseId = model.CourseId,
                GroupId = model.GroupId
            };


            await _unitOfWork.Repository<StudentCourseGroup>().AddAsync(studentCourseGroup);
            await _unitOfWork.CompleteAsync();

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
    }
}
