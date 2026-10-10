using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPolice.Infrastructure.Models
{
    [Table("DigitalEvidences")]
    public class DigitalEvidenceModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string HashSha256 { get; set; }
        public long SizeBytes { get; set; }

        // зовнішній ключ для зв'язку 1-до-багатьох
        public int CyberCaseId { get; set; }

        [ForeignKey("CyberCaseId")]
        public virtual CyberCaseModel CyberCase { get; set; }
    }
}