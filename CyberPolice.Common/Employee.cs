using System;

namespace CyberPolice.Common
{
    public class Employee
    {
        public static int TotalEmployees;

        static Employee()
        {
            TotalEmployees = 0;
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Rank { get; set; }
        public DateTime HireDate { get; set; }

        public Employee(string fullName, string rank)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Rank = rank;
            HireDate = DateTime.Now;
            TotalEmployees++;
        }

        public virtual string GetInfo()
        {
            return FullName + " (" + Rank + ")";
        }

        public static int GetTotalEmployees()
        {
            return TotalEmployees;
        }
    }
}