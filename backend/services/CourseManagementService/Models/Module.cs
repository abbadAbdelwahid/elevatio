using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementService.Models;

public class Module
{
    public int ModuleId { get; set; } // Primary Key
    public string ModuleName { get; set; }
    public string ModuleDescription { get; set; }
    public int ModuleDuration { get; set; } // en heures
    public int FiliereId { get; set; } // Foreign Key vers la table Filiere
    public int TeacherId { get; set; } // Foreign Key vers la table Teacher du microservice Authentification
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "active"; // État du module
    public bool Evaluated  { get; set; } = false; // Optionnel: module obligatoire ou non

    // Relation avec la table Filiere
    public Filiere Filiere { get; set; }

}