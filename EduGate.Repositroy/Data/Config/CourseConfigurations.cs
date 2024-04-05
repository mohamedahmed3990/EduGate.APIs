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
    internal class CourseConfigurations : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(C => C.CourseName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(C => C.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(C => C.Code )
                .IsUnique();
                
        }
    }
}
