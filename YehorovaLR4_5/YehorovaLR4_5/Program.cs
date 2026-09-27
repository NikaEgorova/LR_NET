using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace yehorovaLR4_5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Встановлюємо UTF-8 для коректного відображення тексту в консолі Windows
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Програма аналізу тексту за допомогою колекцій та LINQ ===");
            Console.WriteLine("Розробила: студентка групи 7.F1.25 Єгорова В.С.\n");

            var path = "tmp/text.txt";
            var dic = new SortedDictionary<string, int>();

            // Створюємо тестовий файл автоматично, якщо його не існує, щоб програма не падала
            FileInfo file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            if (!file.Exists)
            {
                File.WriteAllText(path, "Привіт, світе. Це тестовий рядок... King і Edward прийшли в гості. Тут є кілька точок.");
            }

            try
            {
                // Використовуємо конструкцію using для автоматичного закриття потоку файлу
                using (StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Регулярний вираз: вибирає або цілі слова, або окремі розділові знаки
                        var matches = Regex.Matches(line, @"[\w]+|[\p{P}]");

                        foreach (Match match in matches)
                        {
                            string token = match.Value;

                            if (dic.ContainsKey(token))
                                dic[token]++;
                            else
                                dic.Add(token, 1);
                        }
                    }
                }

                // Виведення повного вмісту словника
                Console.WriteLine("--- Вміст файлу (сортований за словником) ---");
                foreach (var it in dic)
                {
                    Console.WriteLine($"\"{it.Key}\": {it.Value}");
                }

                // === LINQ-запит для Точки ===
                Console.WriteLine("\n--- Результат LINQ-запиту (Варіант 1 — Частота точок) ---");

                var dotSelection = dic.Where(val => val.Key == ".");

                if (dotSelection.Any())
                {
                    foreach (var it in dotSelection)
                    {
                        Console.WriteLine($"Символ '{it.Key}' використано в тексті таку кількість разів: {it.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("Символ '.' (крапка) у заданому тексті не знайдений.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Помилка роботи з файлом: {e.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}