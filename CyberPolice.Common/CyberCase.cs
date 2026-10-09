using System;

namespace CyberPolice.Common
{
    public class CyberCase
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime OpenedDate { get; set; }

        public CyberCase(string title)
        {
            Id = Guid.NewGuid();
            Title = title;
            Status = "Відкрито";
            OpenedDate = DateTime.Now;
        }

        public void CloseCase()
        {
            Status = "Закрито";
        }
    }
}