using System;
using CyberPolice.Common;

namespace CyberPolice.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            var caseService = new CrudService<CyberCase>();

            var investigator = new Investigator("Мороз Євгеній", "Лейтенант", "Кіберзлочини");
            investigator.CaseAssigned += title =>
                System.Console.WriteLine("Справу '" + title + "' призначено слідчому " + investigator.FullName);

            var case1 = new CyberCase("Атака на банківський сервер");
            caseService.Create(case1);
            investigator.AssignCase(case1.Title);

            System.Console.WriteLine("Усі справи:");
            foreach (var c in caseService.ReadAll())
                System.Console.WriteLine("- " + c.Title + " (" + c.Status + "), протерміновано: " + c.IsOverdue());

            System.Console.WriteLine("Всього співробітників: " + Employee.GetTotalEmployees());

            System.Console.ReadKey();
        }
    }
}