using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPolice.Infrastructure.Models
{
    [Table("Analysts")]
    public class AnalystModel : EmployeeModel
    {
        public string ToolExpertise { get; set; }
        public int EvidenceAnalyzed { get; set; }
    }
}