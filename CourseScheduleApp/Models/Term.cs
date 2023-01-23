using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CourseScheduleApp.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string TermName { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }


    }
}
