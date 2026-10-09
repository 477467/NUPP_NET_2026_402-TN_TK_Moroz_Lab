using System;

namespace CyberPolice.Common
{
    public static class CaseExtensions
    {
        public static bool IsOverdue(this CyberCase cyberCase, int daysLimit = 30)
        {
            return (DateTime.Now - cyberCase.OpenedDate).TotalDays > daysLimit
                   && cyberCase.Status != "Закрито";
        }
    }
}