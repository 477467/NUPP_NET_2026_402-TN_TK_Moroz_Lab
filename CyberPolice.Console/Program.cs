using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;

namespace CyberPolice.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            using (var context = new CyberPoliceContext())
            {
                var investigatorRepo = new Repository<InvestigatorModel>(context);
                var caseRepo = new Repository<CyberCaseModel>(context);
                var evidenceRepo = new Repository<DigitalEvidenceModel>(context);

                var investigatorService = new CrudServiceDb<InvestigatorModel>(investigatorRepo);
                var caseService = new CrudServiceDb<CyberCaseModel>(caseRepo);
                var evidenceService = new CrudServiceDb<DigitalEvidenceModel>(evidenceRepo);

                // Тестові дані додаються лише в порожню базу
                if (!context.CyberCases.Any())
                {
                    // === Створення слідчого ===
                    var investigator = new InvestigatorModel
                    {
                        FullName = "Мороз Євгеній",
                        Rank = "Лейтенант",
                        HireDate = DateTime.Now,
                        Specialization = "Кіберзлочини",
                        CasesSolved = 0
                    };
                    await investigatorService.CreateAsync(investigator);

                    // === Створення справи ===
                    var cyberCase = new CyberCaseModel
                    {
                        Title = "Атака на банківський сервер",
                        Status = "Відкрито",
                        OpenedDate = DateTime.Now
                    };
                    await caseService.CreateAsync(cyberCase);

                    // === Зв'язок багато-до-багатьох ===
                    cyberCase.Investigators.Add(investigator);
                    await caseService.UpdateAsync(cyberCase);

                    // === Доказ (1-до-багатьох) ===
                    var evidence = new DigitalEvidenceModel
                    {
                        FileName = "server_log.txt",
                        HashSha256 = Guid.NewGuid().ToString("N"),
                        SizeBytes = 204800,
                        CyberCaseId = cyberCase.Id
                    };
                    await evidenceService.CreateAsync(evidence);
                }

                // === Вивід усіх справ ===
                System.Console.WriteLine("Усі справи в базі даних:");
                var allCases = await caseService.ReadAllAsync();
                foreach (var c in allCases)
                    System.Console.WriteLine($"- {c.Title} ({c.Status}), відкрито: {c.OpenedDate}");

                // === Пагінація ===
                System.Console.WriteLine("\nПерша сторінка слідчих (по 5):");
                var page1 = await investigatorService.ReadAllAsync(0, 5);
                foreach (var inv in page1)
                    System.Console.WriteLine($"- {inv.FullName} ({inv.Specialization})");

                // === Доступ через БД навпрямки для перевірки зв'язків ===
                System.Console.WriteLine("\nДокази у першій справі:");
                var caseWithEvidence = context.CyberCases
                    .Include("Evidences")
                    .FirstOrDefault();
                if (caseWithEvidence != null)
                    foreach (var ev in caseWithEvidence.Evidences)
                        System.Console.WriteLine($"- {ev.FileName} ({ev.SizeBytes} байт)");

                System.Console.WriteLine("\nБаза даних: (localdb)\\MSSQLLocalDB, CyberPoliceDb");
            }

            System.Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            System.Console.ReadKey();
        }
    }
}