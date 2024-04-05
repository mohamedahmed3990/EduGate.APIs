using EduGate.APIs.Errors;
using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EduGate.APIs.Controllers
{
    public class StudentsController : BaseApiController
    {
        private readonly IGenaricRepository<Student> _studentRepo;

        public StudentsController(IGenaricRepository<Student> studentRepo)
        {
            _studentRepo = studentRepo;
        }



        [HttpPost]
        public  ActionResult<ApiResponse> AddStudent(Student student)
        {
            try
            {
                var existingStudent = _studentRepo.GetByIdAsync(student.Id).Result;

                if (existingStudent != null)               
                    return BadRequest(new ApiResponse(400, "Student already exists"));

                _studentRepo.Add(student);
                return Ok(new ApiResponse(200, "Student added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {


            var student = await _studentRepo.GetByIdAsync(id);
            if (student is null) { return BadRequest(new ApiResponse(400)); }
            return Ok(student);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {

            var student = await _studentRepo.GetAllAsync();
            if (student is null) { return NotFound(new ApiResponse(404)); }
            return Ok(student);
        }


        [HttpPost("delete")]
        public ActionResult<ApiResponse> DeleteStudent(int id)
        {
            try
            {
                var existingStudent = _studentRepo.GetByIdAsync(id).Result;

                if (existingStudent is null)
                    return BadRequest(new ApiResponse(400, "Student Not exists"));

                _studentRepo.Delete(existingStudent);
                return Ok(new ApiResponse(200, "Student deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpPost("update")]
        public ActionResult<Student> UpdateStudnt(Student student)
        {
            try
            {
                _studentRepo.Update(student);
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ApiResponse(400));
            }
        }
    }
}
