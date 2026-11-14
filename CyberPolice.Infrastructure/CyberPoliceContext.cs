using CyberPolice.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberPolice.Infrastructure
{
    public class CyberPoliceContext : IdentityDbContext<ApplicationUser>
    {
        public CyberPoliceContext(DbContextOptions<CyberPoliceContext> options) : base(options)
        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<InvestigatorModel> Investigators { get; set; }
        public DbSet<AnalystModel> Analysts { get; set; }
        public DbSet<EmployeeCredentialsModel> EmployeeCredentials { get; set; }
        public DbSet<CyberCaseModel> CyberCases { get; set; }
        public DbSet<DigitalEvidenceModel> DigitalEvidences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table-per-Type наслідування
            modelBuilder.Entity<EmployeeModel>().UseTptMappingStrategy();
            modelBuilder.Entity<EmployeeModel>().ToTable("Employees");
            modelBuilder.Entity<InvestigatorModel>().ToTable("Investigators");
            modelBuilder.Entity<AnalystModel>().ToTable("Analysts");

            // 1-до-1: Employee <-> EmployeeCredentials
            modelBuilder.Entity<EmployeeModel>()
                .HasOne(e => e.Credentials)
                .WithOne(c => c.Employee)
                .HasForeignKey<EmployeeCredentialsModel>(c => c.EmployeeId);

            // 1-до-багатьох: CyberCase -> DigitalEvidence
            modelBuilder.Entity<CyberCaseModel>()
                .HasMany(c => c.Evidences)
                .WithOne(e => e.CyberCase)
                .HasForeignKey(e => e.CyberCaseId);

            // багато-до-багатьох: Investigator <-> CyberCase
            modelBuilder.Entity<InvestigatorModel>()
                .HasMany(i => i.Cases)
                .WithMany(c => c.Investigators)
                .UsingEntity<Dictionary<string, object>>(
                    "InvestigatorCases",
                    j => j.HasOne<CyberCaseModel>().WithMany().HasForeignKey("CyberCaseId"),
                    j => j.HasOne<InvestigatorModel>().WithMany().HasForeignKey("InvestigatorId"));
        }
    }
}