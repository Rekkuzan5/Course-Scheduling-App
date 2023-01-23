using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CourseScheduleApp.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int TermID { get; set; } //Foreign Key
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Notes { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public bool NotificationStart { get; set; }
        public bool NotificationEnd { get; set; }


    }
}
