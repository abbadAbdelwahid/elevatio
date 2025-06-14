using EvaluationService.Models;
using EvaluationService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EvaluationService.Data;

public class EvaluationsDbContext : DbContext
{
    public EvaluationsDbContext(DbContextOptions<EvaluationsDbContext> opts)
        : base(opts) { }

    public DbSet<Questionnaire> Questionnaires { get; set; }
    public DbSet<StandardQuestion> StandardQuestions { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Cascade delete : questionnaire → questions → answers
        mb.Entity<Question>()
            .HasOne(q => q.Questionnaire)
            .WithMany(qn => qn.Questions)
            .HasForeignKey(q => q.QuestionnaireId)
            .OnDelete(DeleteBehavior.Cascade);

        mb.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Check constraint pour Score et EvaluatedValue
        mb.Entity<Answer>().ToTable(t=>
            t.HasCheckConstraint("CK_Answer_Response",
                "NOT(\"RawAnswer\" IS NULL AND \"RatingAnswer\" IS NULL)"));

        mb.Entity<Evaluation>().ToTable(t=>
            t.HasCheckConstraint("CK_Evaluation_Score",
                "\"Score\" >= 1 AND \"Score\" <= 5"));

        // Questionnaire : soit FiliereId soit ModuleId non null, pas les deux
        mb.Entity<Questionnaire>().ToTable(t=>
            t.HasCheckConstraint("CK_Questionnaire_Type",
                "NOT(\"FiliereId\" IS NULL AND \"ModuleId\" IS NULL)"));
       
        base.OnModelCreating(mb);
    }
}

