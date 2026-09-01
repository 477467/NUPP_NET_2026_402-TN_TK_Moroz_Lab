namespace CyberPolice.Common
{
    public class Analyst : Employee
    {
        public string ToolExpertise { get; set; }
        public int EvidenceAnalyzed { get; set; }

        public Analyst(string fullName, string rank, string toolExpertise)
            : base(fullName, rank)
        {
            ToolExpertise = toolExpertise;
            EvidenceAnalyzed = 0;
        }

        public void AnalyzeEvidence()
        {
            EvidenceAnalyzed++;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + " — аналітик, інструменти: " + ToolExpertise;
        }
    }
}