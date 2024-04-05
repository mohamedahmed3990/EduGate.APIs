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
    internal class AttendanceConfigurations : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.Property(a => a.Attend).HasDefaultValue(false);

            builder.HasIndex(a => new { a.StudentId, a.CourseGroupId, a.LectureNumber })
                   .IsUnique();

            builder.HasOne(A => A.Student)
                .WithMany();   
            
            builder.HasOne(A => A.CourseGroup)
                .WithMany();    
        }
    }
}
