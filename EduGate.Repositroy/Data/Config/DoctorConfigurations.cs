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
    internal class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.Name).IsRequired();
            builder.Property(d => d.UserName).IsRequired();


            //builder.HasOne(d => d.User)
            //       .WithOne(u => u.Doctor)
            //       .HasForeignKey<Doctor>(d => d.User);
        }
    }
}
