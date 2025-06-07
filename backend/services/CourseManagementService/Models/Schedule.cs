using System;
using System.Collections.Generic;

namespace CourseManagementService.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; } // Primary Key
        public string Group { get; set; } // GINF2, etc.
        public string Year { get; set; }
        public string Week { get; set; }

        // Liste des cours pour cet emploi du temps
        public List<CourseSchedule> Courses { get; set; }
    }

    public class CourseSchedule
    {
        public int CourseScheduleId { get; set; } // Primary Key
        public string Day { get; set; } // Lundi, Mardi, etc.
        public string Start { get; set; } // 08:00
        public string End { get; set; } // 10:00
        public int ModuleId { get; set; } // Référence au Module
        public string Location { get; set; } // Salle B12, etc.

        public Module Module { get; set; } // Relation avec le module
        public int ScheduleId { get; set; } // Relation avec Schedule
        public Schedule Schedule { get; set; } // Relation inverse avec Schedule
    }
}