namespace CyberPolice.Common
{
    public class Investigator : Employee
    {
        public string Specialization { get; set; }
        public int CasesSolved { get; set; }

        public delegate void CaseAssignedHandler(string caseTitle);
        public event CaseAssignedHandler CaseAssigned;

        public Investigator(string fullName, string rank, string specialization)
            : base(fullName, rank)
        {
            Specialization = specialization;
            CasesSolved = 0;
        }

        public void AssignCase(string caseTitle)
        {
            if (CaseAssigned != null)
                CaseAssigned(caseTitle);
        }

        public override string GetInfo()
        {
            return base.GetInfo() + " — слідчий, спеціалізація: " + Specialization;
        }
    }
}