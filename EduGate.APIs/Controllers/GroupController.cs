using EduGate.APIs.Errors;
using EduGate.Core;
using EduGate.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.APIs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var group = await _unitOfWork.Repository<Group>().GetByIdAsync(id);

            if (group is null) return NotFound(new ApiResponse(404));
            return Ok(group);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            var groups = await _unitOfWork.Repository<Group>().GetAllAsync();
            return Ok(groups);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddGroup(Group group)
        {
            try
            {
               
                await _unitOfWork.Repository<Group>().AddAsync(group);
                await _unitOfWork.CompleteAsync();


                return Ok(new ApiResponse(200, "Group added successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }

        }


        [HttpPost("delete")]
        public async Task<ActionResult<ApiResponse>> DeleteGroup(int id)
        {
            try
            {
                var existingGroup = await _unitOfWork.Repository<Group>().GetByIdAsync(id);

                if (existingGroup is null)
                    return BadRequest(new ApiResponse(400, "Group Not exists"));

                _unitOfWork.Repository<Group>().Delete(existingGroup);
                await _unitOfWork.CompleteAsync();

                return Ok(new ApiResponse(200, "Group deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"Internal server error: {ex.Message}"));
            }
        }


        [HttpPost("update")]
        public async Task<ActionResult<Group>> UpdateGroup(Group group)
        {
            _unitOfWork.Repository<Group>().Update(group);
            await _unitOfWork.CompleteAsync();

            return Ok(group);
        }
    }
}
