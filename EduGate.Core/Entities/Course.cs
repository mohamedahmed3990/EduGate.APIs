using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Course : BaseEntity
    {
        public string Code { get; set; }
        public string CourseName { get; set; }

    }
}
