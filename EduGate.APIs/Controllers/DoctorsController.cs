using EduGate.APIs.Errors;
using EduGate.Core;
using EduGate.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.APIs.Controllers
{
    public class DoctorsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);

            if (doctor is null) return NotFound(new ApiResponse(404));
            return Ok(doctor);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync();
            if (doctors is null) return NotFound(new ApiResponse(404));
            return Ok(doctors);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddDoctor(Doctor doctor)
        {
            try
            {
                var existingDoctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(doctor.Id);

                if (existingDoctor != null)
                    return BadRequest(new ApiResponse(400, "Doctor already exists"));

                await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
                await _unitOfWork.CompleteAsync();


                return Ok(new ApiResponse(200, "Doctor added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }

        }


        [HttpPost("delete")]
        public async Task<ActionResult<ApiResponse>> DeleteDoctor(int id)
        {
            try
            {
                var existingDoctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);

                if (existingDoctor is null)
                    return BadRequest(new ApiResponse(400, "Doctor Not exists"));

                _unitOfWork.Repository<Doctor>().Delete(existingDoctor);
                await _unitOfWork.CompleteAsync();

                return Ok(new ApiResponse(200, "Doctor deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<Doctor>> UpdateDoctor(Doctor doctor)
        {
            _unitOfWork.Repository<Doctor>().Update(doctor);
            await _unitOfWork.CompleteAsync();

            return Ok(doctor);
        }
    }
}
