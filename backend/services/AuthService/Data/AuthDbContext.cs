using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthService.Models;

namespace AuthService.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Enseignant> Enseignants => Set<Enseignant>();
    public DbSet<Etudiant> Etudiants => Set<Etudiant>();
    public DbSet<ExternalEvaluator> ExternalEvaluators => Set<ExternalEvaluator>();

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // — Table-Per-Type —
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers");   // table Identity standard
        builder.Entity<Enseignant>().ToTable("Enseignants");        // table spécifique
        builder.Entity<Etudiant>().ToTable("Etudiants"); // 👈 nouveau
        builder.Entity<ExternalEvaluator>().ToTable("ExternalEvaluators");

        
        builder.Entity<Etudiant>()
            .Property(e => e.FiliereId)
            .IsRequired();
        builder.Entity<ExternalEvaluator>().Property(e => e.Organisation).IsRequired();
        builder.Entity<ExternalEvaluator>().Property(e => e.Domaine).IsRequired();
    }
}
