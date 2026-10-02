using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPolice.Infrastructure.Models
{
    [Table("Employees")]
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Rank { get; set; }
        public DateTime HireDate { get; set; }

        // зв'язок 1-до-1
        public virtual EmployeeCredentialsModel Credentials { get; set; }
    }
}