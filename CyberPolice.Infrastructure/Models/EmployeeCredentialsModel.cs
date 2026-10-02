using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPolice.Infrastructure.Models
{
    [Table("EmployeeCredentials")]
    public class EmployeeCredentialsModel
    {
        [Key, ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public virtual EmployeeModel Employee { get; set; }
    }
}