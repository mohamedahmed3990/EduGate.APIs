﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class StudentCourseGroup : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int DoctorCourseGroupId { get; set; }
        public DoctorCourseGroup DoctorCourseGroup { get; set; }
    }
}