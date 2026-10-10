using System;

namespace CyberPolice.REST.Models
{
    public class InvestigatorApiModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Rank { get; set; }
        public string Specialization { get; set; }
        public int CasesSolved { get; set; }
    }

    public class InvestigatorCreateModel
    {
        public string FullName { get; set; }
        public string Rank { get; set; }
        public string Specialization { get; set; }
    }
}