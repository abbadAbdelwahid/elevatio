using Microsoft.EntityFrameworkCore;
using CourseManagementService.Models;

namespace CourseManagementService;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Filiere> Filieres { get; set; }
    public DbSet<Module> Modules { get; set; }
    
    // /Data/ApplicationDbContext.cs
    public DbSet<Note> Notes { get; set; }


    // Méthode pour configurer le modèle de données (si nécessaire)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de la relation entre Filiere et Module
        modelBuilder.Entity<Module>()
            .HasOne(m => m.Filiere)
            .WithMany(f => f.Modules)
            .HasForeignKey(m => m.FiliereId);

        // Ajouter d'autres configurations si nécessaire (par exemple, validation de champs, etc.)
    }
}