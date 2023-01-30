using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseScheduleApp.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement] 
        public int ID { get; set; }
        public int CourseID { get; set; }   // Foreign Key
        public string AssessmentName { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set; }
        public bool Notification { get; set; }

    }
}
