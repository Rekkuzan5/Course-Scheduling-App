using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CourseScheduleApp.Models
{
    class Widget
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int GadgetId { get; set; } // FKey from Gadget
        public string Name { get; set; }
        public string Color { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public bool StartNotification { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
    }
}
