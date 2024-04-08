using EduGate.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.APIs.Controllers
{
    public class StudentCourseGroupController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentCourseGroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


    }
}
