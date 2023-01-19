using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CourseScheduleApp.Models
{
    class Gadget
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
