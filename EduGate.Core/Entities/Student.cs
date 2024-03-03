using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public ICollection<Attendence> Attendences { get; set; } = new HashSet<Attendence>();
    }
}
