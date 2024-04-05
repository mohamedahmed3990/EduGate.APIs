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
    internal class CourseGroupConfigurations : IEntityTypeConfiguration<CourseGroup>
    {
        public void Configure(EntityTypeBuilder<CourseGroup> builder)
        {
            builder.HasOne(p => p.Course)
                   .WithMany();

            builder.HasOne(p => p.Group)
                   .WithMany();

            builder.HasIndex(p => new {p.GroupId, p.CourseId})
                   .IsUnique();
        }
    }
}
