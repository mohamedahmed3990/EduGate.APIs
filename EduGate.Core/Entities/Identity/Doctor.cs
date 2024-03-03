using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities.Identity
{
    public class Doctor : IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();

    }
}
