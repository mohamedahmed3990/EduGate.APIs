using AutoMapper;
using EduGate.APIs.DTOs;
using EduGate.Core.Entities;

namespace EduGate.APIs.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentCourseGroup, studentCourseToReturnDto>()
                .ForMember(d => d.StudentName, o => o.MapFrom(s => s.Student.Name))
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.CourseName))
                .ForMember(d => d.GroupName, o => o.MapFrom(s => s.Group.GroupName));
            
            
            CreateMap<StudentCourseGroup, SpecificCourseGroupForStudentToReturnDto>()
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.CourseName))
                .ForMember(d => d.GroupName, o => o.MapFrom(s => s.Group.GroupName));


            CreateMap<DoctorCourseGroup, DoctorCourseGroupToReturnDto>()
               .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor.Name))
               .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.CourseName))
               .ForMember(d => d.GroupName, o => o.MapFrom(s => s.Group.GroupName));
        }
    }
}
