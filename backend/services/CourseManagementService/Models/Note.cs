// /Models/Note.cs
using System;

namespace CourseManagementService.Models
{
    public class Note
    {
        public int NoteId { get; set; }  // Clé primaire

        public int ModuleId { get; set; }  // Clé étrangère vers le module
        public Module Module { get; set; }  // Navigation property pour Module

        public int StudentId { get; set; }  // ID de l'étudiant (clé étrangère vers la table des étudiants)

        public int Grade { get; set; }  // La note (entre 0 et 20)
        
        public string Comment { get; set; }  // Commentaire

        public DateTime CreatedAt { get; set; }  // Date de création
        public DateTime UpdatedAt { get; set; }  // Date de mise à jour
    }
}