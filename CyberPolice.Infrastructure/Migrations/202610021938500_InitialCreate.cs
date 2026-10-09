namespace CyberPolice.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Rank = c.String(),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeCredentials",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false),
                        Login = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.CyberCases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Status = c.String(),
                        OpenedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DigitalEvidences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        HashSha256 = c.String(),
                        SizeBytes = c.Long(nullable: false),
                        CyberCaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CyberCases", t => t.CyberCaseId, cascadeDelete: true)
                .Index(t => t.CyberCaseId);
            
            CreateTable(
                "dbo.InvestigatorCases",
                c => new
                    {
                        InvestigatorId = c.Int(nullable: false),
                        CyberCaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InvestigatorId, t.CyberCaseId })
                .ForeignKey("dbo.Investigators", t => t.InvestigatorId, cascadeDelete: true)
                .ForeignKey("dbo.CyberCases", t => t.CyberCaseId, cascadeDelete: true)
                .Index(t => t.InvestigatorId)
                .Index(t => t.CyberCaseId);
            
            CreateTable(
                "dbo.Analysts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ToolExpertise = c.String(),
                        EvidenceAnalyzed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Investigators",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Specialization = c.String(),
                        CasesSolved = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Investigators", "Id", "dbo.Employees");
            DropForeignKey("dbo.Analysts", "Id", "dbo.Employees");
            DropForeignKey("dbo.InvestigatorCases", "CyberCaseId", "dbo.CyberCases");
            DropForeignKey("dbo.InvestigatorCases", "InvestigatorId", "dbo.Investigators");
            DropForeignKey("dbo.DigitalEvidences", "CyberCaseId", "dbo.CyberCases");
            DropForeignKey("dbo.EmployeeCredentials", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Investigators", new[] { "Id" });
            DropIndex("dbo.Analysts", new[] { "Id" });
            DropIndex("dbo.InvestigatorCases", new[] { "CyberCaseId" });
            DropIndex("dbo.InvestigatorCases", new[] { "InvestigatorId" });
            DropIndex("dbo.DigitalEvidences", new[] { "CyberCaseId" });
            DropIndex("dbo.EmployeeCredentials", new[] { "EmployeeId" });
            DropTable("dbo.Investigators");
            DropTable("dbo.Analysts");
            DropTable("dbo.InvestigatorCases");
            DropTable("dbo.DigitalEvidences");
            DropTable("dbo.CyberCases");
            DropTable("dbo.EmployeeCredentials");
            DropTable("dbo.Employees");
        }
    }
}
