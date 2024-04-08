using EduGate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Repositroy.Data.Config
{
    internal class DoctorCourseGroupConfigurations : IEntityTypeConfiguration<DoctorCourseGroup>
    {
        public void Configure(EntityTypeBuilder<DoctorCourseGroup> builder)
        {
            builder.HasIndex(Dcg => new { Dcg.CourseId, Dcg.GroupId, Dcg.DoctorId })
                   .IsUnique();

            builder.HasOne(Dcg => Dcg.Course)
                   .WithMany();
            
            builder.HasOne(Dcg => Dcg.Group)
                   .WithMany();

            builder.HasOne(Dcg => Dcg.Doctor)
                   .WithMany();
            
        }
    }
}
