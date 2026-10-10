using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPolice.Infrastructure.Models
{
    [Table("CyberCases")]
    public class CyberCaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime OpenedDate { get; set; }

        // зв'язок 1-до-багатьох
        public virtual ICollection<DigitalEvidenceModel> Evidences { get; set; } = new List<DigitalEvidenceModel>();

        // зв'язок багато-до-багатьох
        public virtual ICollection<InvestigatorModel> Investigators { get; set; } = new List<InvestigatorModel>();
    }
}