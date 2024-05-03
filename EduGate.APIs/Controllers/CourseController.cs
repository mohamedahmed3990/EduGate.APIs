using EduGate.APIs.Errors;
using EduGate.Core.Entities;
using EduGate.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EduGate.APIs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);

            if (course is null) return NotFound(new ApiResponse(404));
            return Ok(course);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            return Ok(courses);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddCourse(Course course)
        {
            try
            {
                var existingCourse = await _unitOfWork.Repository<Course>().GetByIdAsync(course.Id);

                if (existingCourse != null)
                    return BadRequest(new ApiResponse(400, "Course already exists"));

                await _unitOfWork.Repository<Course>().AddAsync(course);
                await _unitOfWork.CompleteAsync();


                return Ok(new ApiResponse(200, "Course added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }

        }


        [HttpPost("delete/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCourse([FromRoute]int id)
        {
            try
            {
                var existingCourse = await _unitOfWork.Repository<Course>().GetByIdAsync(id);

                if (existingCourse is null)
                    return BadRequest(new ApiResponse(400, "Course Not exists"));

                _unitOfWork.Repository<Course>().Delete(existingCourse);
                await _unitOfWork.CompleteAsync();

                return Ok(new ApiResponse(200, "Course deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<Course>> UpdateCourse(Course course)
        {
            _unitOfWork.Repository<Course>().Update(course);
            await _unitOfWork.CompleteAsync();

            return Ok(course);
        }
    }
}
