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

namespace EduGate.APIs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IMapper _mapper;

        public DoctorsController(IUnitOfWork unitOfWork, IDoctorRepository doctorRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _doctorRepo = doctorRepo;
            _mapper = mapper;
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
                var existDoctor = await _doctorRepo.GetbyName(doctor.Name);

                if (existDoctor is not null)
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





        [HttpPost("courseToDoctor")]
        public async Task<ActionResult> AddCourseToDoctor(DoctorCourseGorupModel model)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(model.DoctorId);
            if (doctor is null)
                return NotFound(new ApiResponse(404));

            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(model.CourseId);
            if (course is null)
                return NotFound(new ApiResponse(404));

            var group = await _unitOfWork.Repository<Group>().GetByIdAsync(model.GroupId);
            if (group is null)
                return NotFound(new ApiResponse(404));

            var spec = new DoctorCourseSepcs(model.DoctorId, model.CourseId, model.GroupId);

            var doctorCourse = await _unitOfWork.Repository<DoctorCourseGroup>().GetByIdWithSpecAsync(spec);

            if (doctorCourse is not null)
            {
                return BadRequest(new ApiResponse(400, "doctor aleady exist!!"));
            }

            var doctorCourseGroup = new DoctorCourseGroup()
            {
                DoctorId = model.DoctorId,
                CourseId = model.CourseId,
                GroupId = model.GroupId
            };


            await _unitOfWork.Repository<DoctorCourseGroup>().AddAsync(doctorCourseGroup);
            await _unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(200, "doctor added to course group successfully"));


        }



        [HttpGet("doctorCourse")]
        public async Task<ActionResult> GetAllDoctorCourseGroup()
        {
            var spec = new DoctorCourseSepcs();

            var doctors = await _unitOfWork.Repository<DoctorCourseGroup>().GetAllWithSpecAsync(spec);

            //var doctorMapped = _mapper.Map<IEnumerable<DoctorCourseGroup>, IEnumerable<DoctorCourseGroupToReturnDto>>(doctors);

            var formattedOutput = doctors
            .GroupBy(sc => new { sc.DoctorId, sc.Doctor.Name }) 
            .Select(group => new
            {
                DoctorId = group.Key.DoctorId,
                Doctor = group.Key.Name,            
                courses = group.Select(sc => new
                {
                    coursename = sc.Course.CourseName,
                    group = sc.Group.GroupName
                }).ToList()
            });
            return Ok(formattedOutput);
          
        }
    }
}
