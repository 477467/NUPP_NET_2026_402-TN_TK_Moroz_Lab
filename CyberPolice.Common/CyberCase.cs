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
    public static class CyberCaseFactory
    {
        private static readonly Random _random = new Random();
        private static readonly string[] Titles = {
            "Атака на банківський сервер", "Фішингова розсилка", "DDoS-атака на держреєстр",
            "Витік персональних даних", "Крипто-шахрайство", "Злом облікового запису"
        };

        public static CyberCase CreateNew()
        {
            lock (_random)
            {
                string title = Titles[_random.Next(Titles.Length)] + " #" + _random.Next(1000, 9999);
                return new CyberCase(title);
            }
        }
    }
}