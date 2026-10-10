using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CyberPolice.Common;

namespace CyberPolice.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            string filePath = "cases.json";
            var service = new CrudServiceAsync<CyberCase>(filePath);

            // === Паралельне створення 1000 об'єктів ===
            var bag = new ConcurrentBag<CyberCase>();
            var sw = Stopwatch.StartNew();

            Parallel.For(0, 1000, i =>
            {
                var newCase = CyberCaseFactory.CreateNew();
                bag.Add(newCase);
                service.CreateAsync(newCase).Wait();
            });

            sw.Stop();
            System.Console.WriteLine($"Створено {bag.Count} справ за {sw.ElapsedMilliseconds} мс");

            // === LINQ: Min/Max/Average ===
            var allCases = await service.ReadAllAsync();
            var casesList = allCases.ToList();

            var daysOpen = casesList.Select(c => (DateTime.Now - c.OpenedDate).TotalSeconds);
            System.Console.WriteLine($"Мін. час відкриття (с): {daysOpen.Min():F4}");
            System.Console.WriteLine($"Макс. час відкриття (с): {daysOpen.Max():F4}");
            System.Console.WriteLine($"Середній час відкриття (с): {daysOpen.Average():F4}");

            // === Пагінація ===
            var page1 = await service.ReadAllAsync(0, 10);
            System.Console.WriteLine("\nПерша сторінка (10 елементів):");
            foreach (var c in page1)
                System.Console.WriteLine($"- {c.Title}");

            // === Збереження у файл ===
            bool saved = await service.SaveAsync();
            System.Console.WriteLine($"\nЗбережено у файл: {saved}, шлях: {filePath}");

            // === Приклади примітивів синхронізації ===
            DemoLock();
            await DemoSemaphore();
            DemoAutoResetEvent();

            System.Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            System.Console.ReadKey();
        }

        // === lock ===
        private static readonly object _lockObj = new object();
        private static int _counter = 0;

        static void DemoLock()
        {
            System.Console.WriteLine("\n=== Демонстрація lock ===");
            Parallel.For(0, 1000, i =>
            {
                lock (_lockObj)
                {
                    _counter++;
                }
            });
            System.Console.WriteLine($"Лічильник після 1000 потоків (lock): {_counter}");
        }

        // === SemaphoreSlim ===
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(2); // максимум 2 потоки одночасно

        static async Task DemoSemaphore()
        {
            System.Console.WriteLine("\n=== Демонстрація Semaphore (макс. 2 одночасно) ===");
            var tasks = Enumerable.Range(1, 5).Select(async i =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    System.Console.WriteLine($"Потік {i} увійшов у критичну секцію");
                    await Task.Delay(300);
                    System.Console.WriteLine($"Потік {i} вийшов");
                }
                finally
                {
                    _semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }

        // === AutoResetEvent ===
        static void DemoAutoResetEvent()
        {
            System.Console.WriteLine("\n=== Демонстрація AutoResetEvent ===");
            var autoEvent = new AutoResetEvent(false);

            var producer = new Thread(() =>
            {
                System.Console.WriteLine("Виробник: готує дані...");
                Thread.Sleep(500);
                System.Console.WriteLine("Виробник: дані готові, сигналізує споживачу");
                autoEvent.Set();
            });

            var consumer = new Thread(() =>
            {
                System.Console.WriteLine("Споживач: чекає на сигнал...");
                autoEvent.WaitOne();
                System.Console.WriteLine("Споживач: отримав сигнал, обробляє дані");
            });

            producer.Start();
            consumer.Start();
            producer.Join();
            consumer.Join();
        }
    }
}