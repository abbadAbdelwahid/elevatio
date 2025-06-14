using System;
using System.Collections.Generic;

namespace CourseManagementService.Models;

public class Filiere
{
    public int FiliereId { get; set; } // Primary Key
    public string FiliereName { get; set; } 
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Relation avec la table Module
    public ICollection<Module> Modules { get; set; }

    // Constructor
    public Filiere()
    {
        Modules = new List<Module>();
    }
}