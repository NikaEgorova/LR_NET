using System;

namespace yehorovaLR2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Програма обчислення коефіцієнтів функцій форми скінченних елементів для чотирикутника ===");
            Console.WriteLine("Розробила: студентка групи 7.F1.25 Єгорова В.С.\n");

            Console.WriteLine("--- Введення координат 4-х вершин чотирикутника ---");
            double[,] coordinates = new double[4, 2];

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"\nВузол №{i}:");
                coordinates[i, 0] = ReadDouble($"  Введіть координату X{i}: ");
                coordinates[i, 1] = ReadDouble($"  Введіть координату Y{i}: ");
            }

            Console.WriteLine("\nМатриця коефіцієнтів функцій форми (кожен рядок - окрема функція вузла):");

            // Створення об'єкта класу чотирикутника згідно з ООП ієрархією
            ShapeFunction fe = new ShapeQuad(coordinates);
            fe.Print();

            Console.WriteLine("\nНатисніть будь-яку клавішу ...");
            Console.ReadKey(true);
            Console.WriteLine("\n--------------------------------------------------\n");

        }

        // Перевірений безпечний метод читання дійсних чисел
        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value))
                {
                    return value;
                }
                Console.WriteLine("  Помилка: Некоректний формат числа! Спробуйте ще раз.");
            }
        }
    }


    // АБСТРАКТНИЙ БАЗОВИЙ КЛАС
    abstract class ShapeFunction
    {
        protected int Size;      // Кількість коефіцієнтів функції форми
        protected int Dim;       // Розмірність простору
        protected double[,] C;   // Матриця шуканих коефіцієнтів
        protected double[,] X;   // Матриця координат вершин СЕ

        protected ShapeFunction()
        {
            Size = Dim = 0;
        }

        protected void SetCoord(int psize, double[,] px)
        {
            Size = psize;
            X = new double[Size, Dim];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Dim; j++)
                    X[i, j] = px[i, j];

            Create();
        }

        protected abstract double ShapeCoeff(int i, int j);
        // Метод розв'язання СЛАР методом Гаусса з лабораторної роботи
        private bool Solve(double[,] matr, double[] result, double eps = 1.0E-10)
        {
            double coeff;
            for (var i = 0; i < Size - 1; i++)
            {
                if (Math.Abs(matr[i, i]) < eps)
                    continue;
                for (var j = i + 1; j < Size; j++)
                {
                    if (Math.Abs(coeff = matr[j, i]) < eps)
                        continue;
                    for (var k = i; k < Size + 1; k++)
                        matr[j, k] -= (coeff * matr[i, k] / matr[i, i]);
                }
            }
            if (Math.Abs(matr[Size - 1, Size - 1]) < eps)
                return false;

            result[Size - 1] = matr[Size - 1, Size] / matr[Size - 1, Size - 1];
            for (int k = 0; k < Size - 1; k++)
            {
                int i = Size - k - 2;
                var sum = matr[i, Size];

                for (int j = i + 1; j < Size; j++)
                    sum -= result[j] * matr[i, j];
                if (Math.Abs(matr[i, i]) < eps)
                    return false;
                result[i] = sum / matr[i, i];
            }
            return true;
        }

        // Обчислення матриці коефіцієнтів C шляхом послідовного формування СЛАР для кожного вузла
        public void Create()
        {
            double[,] A = new double[Size, Size + 1];
            double[] res = new double[Size];
            C = new double[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                        A[j, k] = ShapeCoeff(j, k);

                    // i-та функція форми дорівнює 1 в i-му вузлі і 0 в інших
                    A[j, Size] = (i == j) ? 1.0 : 0.0;
                }

                if (!Solve(A, res))
                    Console.WriteLine("Помилка: Вироджений скінченний елемент (Bad FE)!");

                for (int j = 0; j < Size; j++)
                    C[i, j] = res[j];
            }
        }

        public void Print()
        {
            for (int i = 0; i < Size; i++)
            {
                Console.Write($"Вузол {i}: ");
                for (int j = 0; j < Size; j++)
                    Console.Write("{0:F4} \t", C[i, j]);
                Console.WriteLine();
            }
        }
    }


    // ПОХІДНИЙ КЛАС ДЛЯ ВАРІАНТА ЧОТИРИКУТНИКА
    class ShapeQuad : ShapeFunction
    {
        public ShapeQuad()
        {
            Size = 4; // 4 вузли = 4 невідомі коефіцієнти (c0, c1, c2, c3)
            Dim = 2;  // Двовимірний простір (X, Y)
        }

        public ShapeQuad(double[,] px)
        {
            Size = 4;
            Dim = 2;
            SetCoord(Size, px); // Автоматичний виклик базової ініціалізації та розрахунку
        }

        // Формування базисних поліноміальних доданків: 1, x, y, x*y
        protected override double ShapeCoeff(int i, int j)
        {
            double[] s = { 1.0, X[i, 0], X[i, 1], X[i, 0] * X[i, 1] };
            return s[j];
        }
    }
}