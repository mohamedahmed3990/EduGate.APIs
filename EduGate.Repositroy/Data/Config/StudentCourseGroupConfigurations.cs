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
    internal class StudentCourseGroupConfigurations : IEntityTypeConfiguration<StudentCourseGroup>
    {
        public void Configure(EntityTypeBuilder<StudentCourseGroup> builder)
        {
            builder.HasIndex(Scg => new { Scg.StudentId, Scg.CourseId})
                   .IsUnique();

            builder.HasOne(Scg => Scg.Student)
                   .WithMany();
            
            builder.HasOne(Scg => Scg.Course)
                   .WithMany();

            builder.HasOne(Scg => Scg.Group)
                   .WithMany();

            
        }
    }
}
