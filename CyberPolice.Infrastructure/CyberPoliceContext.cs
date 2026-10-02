using System.Data.Entity;
using CyberPolice.Infrastructure.Models;

namespace CyberPolice.Infrastructure
{
    public class CyberPoliceContext : DbContext
    {
        static CyberPoliceContext()
        {   
            Database.SetInitializer(new CreateDatabaseIfNotExists<CyberPoliceContext>());
        }

        public CyberPoliceContext() : base("name=CyberPoliceContext")
        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<InvestigatorModel> Investigators { get; set; }
        public DbSet<AnalystModel> Analysts { get; set; }
        public DbSet<EmployeeCredentialsModel> EmployeeCredentials { get; set; }
        public DbSet<CyberCaseModel> CyberCases { get; set; }
        public DbSet<DigitalEvidenceModel> DigitalEvidences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>().ToTable("Employees");
            modelBuilder.Entity<InvestigatorModel>().ToTable("Investigators");
            modelBuilder.Entity<AnalystModel>().ToTable("Analysts");

            modelBuilder.Entity<EmployeeModel>()
                .HasOptional(e => e.Credentials)
                .WithRequired(c => c.Employee);

            modelBuilder.Entity<CyberCaseModel>()
                .HasMany(c => c.Evidences)
                .WithRequired(e => e.CyberCase)
                .HasForeignKey(e => e.CyberCaseId);

            modelBuilder.Entity<InvestigatorModel>()
                .HasMany(i => i.Cases)
                .WithMany(c => c.Investigators)
                .Map(m =>
                {
                    m.ToTable("InvestigatorCases");
                    m.MapLeftKey("InvestigatorId");
                    m.MapRightKey("CyberCaseId");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}