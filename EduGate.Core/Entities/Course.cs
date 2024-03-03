using EduGate.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public ICollection<Attendence> Attendences { get; set; } = new HashSet<Attendence>();
    }
}
