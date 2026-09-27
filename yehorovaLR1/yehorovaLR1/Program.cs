using System;

namespace yehorovaLR1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Програма для обчислення площі паралелограма ===\n");
            Console.WriteLine("Розробила: студентка групи 7.F1.25 Єгорова В.С.\n");

            bool work = true;
            while (work)
            {
                Console.WriteLine("Оберіть метод розрахунку:");
                Console.WriteLine("1 — Через основу та висоту (S = a * h)");
                Console.WriteLine("2 — Через дві сторони та кут між ними (S = a * b * sin(α))");
                Console.WriteLine("3 — Через діагоналі та кут між ними (S = 0.5 * d1 * d2 * sin(φ))\n");
                Console.WriteLine("0 — Вийти з програми");
                Console.Write("\nВаш вибір (0, 1, 2 або 3): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CalculateByBaseAndHeight();
                        break;
                    case "2":
                        CalculateBySidesAndAngle();
                        break;
                    case "3":
                        CalculateByDiagonalsAndAngle();
                        break;
                    case "0":
                        work = false;
                        break;
                    default:
                        Console.WriteLine("Помилка: Неправильний вибір. Будь ласка, оберіть 0, 1, 2 або 3.\n");
                        break;
                }
            }
        }

        // УНІВЕРСАЛЬНИЙ МЕТОД ДЛЯ ЧИТАННЯ ТА ВАЛІДАЦІЇ ЧИСЕЛ
        static double ReadDouble(string prompt, double min = 0, double max = double.MaxValue, string customRangeError = null)
        {
            while (true)
            {
                Console.Write(prompt);
                // TryParse перевіряє формат без виклику винятків try-catch
                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    if (value > min && value < max)
                    {
                        return value; // Вертаємо правильне число і виходимо з циклу
                    }

                    // Виведення помилки діапазону
                    Console.WriteLine(customRangeError ?? $"Помилка: Значення має бути більшим за {min}!");
                }
                else
                {
                    Console.WriteLine("Помилка: Введено некоректний формат числа! Використовуйте цифри (і кому для дробів).");
                }
            }
        }

        // Допоміжний метод для паузи після розрахунку
        static void WaitForKey()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey(true);
            Console.WriteLine("\n--------------------------------------------------\n");
        }

        // Метод 1: Через основу та висоту
        static void CalculateByBaseAndHeight()
        {
            double sideA = ReadDouble("Введіть довжину основи (a): ");
            double height = ReadDouble("Введіть висоту (h): ");

            double area = sideA * height;
            Console.WriteLine($"\nРезультат: Площа паралелограма дорівнює {area:F2}");
            WaitForKey();
        }

        // Метод 2: Через дві сторони та кут
        static void CalculateBySidesAndAngle()
        {
            double sideA = ReadDouble("Введіть першу сторону (a): ");
            double sideB = ReadDouble("Введіть другу сторону (b): ");
            double angleDegrees = ReadDouble(
                "Введіть кут між ними в градусах (α): ",
                0, 180,
                "Помилка: Кут повинен бути в межах від 0 до 180 градусів!"
            );
            double angleRadians = angleDegrees * Math.PI / 180.0;
            double area = sideA * sideB * Math.Sin(angleRadians);

            Console.WriteLine($"\nРезультат: Площа паралелограма дорівнює {area:F2}");
            WaitForKey();
        }

        // Метод 3: Через діагоналі та кут
        static void CalculateByDiagonalsAndAngle()
        {
            double d1 = ReadDouble("Введіть першу діагональ (d1): ");
            double d2 = ReadDouble("Введіть другу діагональ (d2): ");
            double angleDegrees = ReadDouble(
                "Введіть кут між діагоналями в градусах (φ): ",
                0, 180,
                "Помилка: Кут повинен бути в межах від 0 до 180 градусів!"
            );

            double angleRadians = angleDegrees * Math.PI / 180.0;
            double area = 0.5 * d1 * d2 * Math.Sin(angleRadians);

            Console.WriteLine($"\nРезультат: Площа паралелограма дорівнює {area:F2}");
            WaitForKey();
        }
    }
}