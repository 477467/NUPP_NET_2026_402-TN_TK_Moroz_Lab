using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPolice.Infrastructure.Models
{
    [Table("Investigators")]
    public class InvestigatorModel : EmployeeModel
    {
        public string Specialization { get; set; }
        public int CasesSolved { get; set; }

        public virtual ICollection<CyberCaseModel> Cases { get; set; } = new List<CyberCaseModel>();
    }
}