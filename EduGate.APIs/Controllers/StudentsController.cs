using EduGate.APIs.Errors;
using EduGate.Core;
using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EduGate.APIs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

            if (student is null) return NotFound(new ApiResponse(404));
            return Ok(student);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            return Ok(students);
        }


        [HttpPost]
        public  async Task<ActionResult<ApiResponse>> AddStudent(Student student)
        {
            try
            {
                var existingStudent = await _unitOfWork.Repository<Student>().GetByIdAsync(student.Id);

                if (existingStudent != null)               
                    return BadRequest(new ApiResponse(400, "Student already exists"));               

                await _unitOfWork.Repository<Student>().AddAsync(student);
                await _unitOfWork.CompleteAsync();
                
               
                return Ok(new ApiResponse(200, "Student added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }

        }


        [HttpPost("delete")]
        public async Task<ActionResult<ApiResponse>> DeleteStudent(int id)
        {
            try
            {
                var existingStudent = await _unitOfWork.Repository<Student>().GetByIdAsync(id);

                if (existingStudent is null)
                    return BadRequest(new ApiResponse(400, "Student Not exists"));

                 _unitOfWork.Repository<Student>().Delete(existingStudent);
                 await _unitOfWork.CompleteAsync();

                return Ok(new ApiResponse(200, "Student deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<Student>> UpdateStudent(Student student)
        {
             _unitOfWork.Repository<Student>().Update(student);
             await _unitOfWork.CompleteAsync();

             return Ok(student);   
        }
    }
}
