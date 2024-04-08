using EduGate.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? UserId { get; set; }
    }
}
