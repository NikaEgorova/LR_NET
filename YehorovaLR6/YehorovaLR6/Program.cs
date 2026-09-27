using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace yehorovaLR6
{
    // Клас для передачі параметрів у потік (діапазон ітерацій)
    class TreadParams
    {
        public int begin, end;
        public TreadParams(int b, int e)
        {
            begin = b;
            end = e;
        }
    }

    class Program
    {
        public static Mutex mutex = new Mutex(); // М'ютекс для синхронізації доступу
        public static double Sum = 0;           // Спільний глобальний ресурс для суми ряду

        static void Main(string[] args)
        {
            // Налаштування локалізації консолі
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Багатопотокове обчислення суми ряду (Варіант 1) ===");
            Console.WriteLine("Розробила: студентка групи 7.F1.25 Єгорова В.С.\n");

            // Задаємо велику кількість ітерацій для наочності зміни часу роботи
            int maxIter = 50000000;
            Console.WriteLine($"Загальна кількість членів ряду (n): {maxIter}\n");

            // Словник для збереження результатів тестування: <Кількість потоків, Час у мс>
            Dictionary<int, long> performanceReport = new Dictionary<int, long>();

            Console.WriteLine("Запуск обчислень для різної кількості потоків...");
            Console.WriteLine("------------------------------------------------");

            // Тестуємо роботу програми від 1 до 12 потоків
            for (int threadsCount = 1; threadsCount <= 12; threadsCount++)
            {
                // Скидаємо суму перед кожним новим тестом
                Sum = 0;

                Thread[] thr = new Thread[threadsCount];
                int step = maxIter / threadsCount;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Запуск потоків
                for (int i = 0; i < threadsCount; i++)
                {
                    thr[i] = new Thread(new ParameterizedThreadStart(CalcSeries));

                    int beginIndex = i * step;
                    // Останній потік забирає залишок ітерацій
                    int endIndex = (i == threadsCount - 1) ? maxIter : (i + 1) * step;

                    thr[i].Start(new TreadParams(beginIndex, endIndex));
                }

                // Очікування завершення всіх потоків
                for (int i = 0; i < threadsCount; i++)
                {
                    thr[i].Join();
                }

                stopwatch.Stop();
                long elapsedMs = stopwatch.ElapsedMilliseconds;

                // Зберігаємо результат для графіка
                performanceReport.Add(threadsCount, elapsedMs);

                Console.WriteLine($"Потоків: {threadsCount,2} | Час: {elapsedMs,4} мс | Сума ряду = {Sum:F10}");
            }

            // Побудова графіка залежності часу роботи від кількості потоків
            DrawAsciiGraph(performanceReport);

            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу з програми...");
            Console.ReadKey();
        }

        // Метод обчислення суми ряду для Варіанта 1 з використанням М'ютексу
        public static void CalcSeries(object param)
        {
            double localSum = 0;

            if (param is TreadParams p)
            {
                // Обчислення локальної суми потоку (ефективний підхід без блокування на кожній ітерації)
                for (double i = p.begin; i < p.end; i++)
                {
                    // Формула Варіанта 1: i / (1 + i^4)
                    localSum += (i / (1.0 + i * i * i * i));
                }

                // Синхронізація доступу та запис у спільне поле
                mutex.WaitOne();
                Sum += localSum;
                mutex.ReleaseMutex();
            }
        }
        // Метод малювання консольного графіка (ASCII-Гістограми)
        static void DrawAsciiGraph(Dictionary<int, long> data)
        {
            Console.WriteLine("\n========================================================");
            Console.WriteLine(" ГРАФІК ЗАЛЕЖНОСТІ ЧАСУ РОБОТИ (мс) ВІД КІЛЬКОСТІ ПОТОКІВ ");
            Console.WriteLine("========================================================\n");

            // Знаходимо максимальний час для масштабування стовпчиків
            long maxTime = 1;
            foreach (var val in data.Values)
            {
                if (val > maxTime) maxTime = val;
            }

            // Малюємо гістограму
            foreach (var item in data)
            {
                Console.Write($"Потоків {item.Key,2} [{item.Value,4} мс]: ");

                // Визначаємо довжину стовпчика (макс. 40 символів)
                int barLength = (int)((double)item.Value / maxTime * 40);

                // Захист від нульового часу
                if (barLength == 0 && item.Value > 0) barLength = 1;

                // Виводимо стовпчик
                for (int i = 0; i < barLength; i++)
                {
                    Console.Write("█");
                }
                Console.WriteLine();
            }
            Console.WriteLine("--------------------------------------------------------");
        }
    }
}