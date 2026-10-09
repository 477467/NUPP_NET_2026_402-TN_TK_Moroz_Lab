using System;

namespace CyberPolice.REST.Models
{
    public class CyberCaseApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime OpenedDate { get; set; }
    }

    public class CyberCaseCreateModel
    {
        public string Title { get; set; }
    }
}