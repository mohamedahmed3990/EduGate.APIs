﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Attendance : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int LectureNumber { get; set; }
        public bool Attend { get; set; }
        public DateTime? Date { get; set; }
    }
}
