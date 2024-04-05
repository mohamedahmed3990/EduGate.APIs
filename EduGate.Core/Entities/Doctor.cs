﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }
}