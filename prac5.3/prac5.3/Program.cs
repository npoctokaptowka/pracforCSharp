using System;
using System.Collections.Generic;

namespace Practice
{
    internal class Program
    {
        static Random random = new Random();

        static int[] RandomArray(int n, int min, int max)
        {
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = random.Next(min, max + 1);
            }
            return arr;
        }

        static int[,] RandomMatrix(int rows, int cols, int min, int max)
        {
            int[,] m = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    m[i, j] = random.Next(min, max + 1);
                }
            }
            return m;
        }

        static void PrintArray(string title, int[] arr)
        {
            Console.WriteLine(title + string.Join(" ", arr));
        }

        static void PrintMatrix(string title, int[,] m)
        {
            Console.WriteLine(title);
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    Console.Write(m[i, j].ToString().PadLeft(5));
                }
                Console.WriteLine();
            }
        }

        static void Task1()
        {
            Console.WriteLine("=== Задача 1: змейка с нижнего правого угла ===");
            Console.Write("Размер массива N: ");
            int n = Convert.ToInt32(Console.ReadLine());

            int[] arr = RandomArray(n, 0, 99);
            PrintArray("Массив: ", arr);
            Array.Sort(arr);
            PrintArray("Отсортированный: ", arr);

            int k = (int)Math.Sqrt(n);
            int[,] m = new int[k, k];
            int index = 0;
            for (int row = k - 1; row >= 0; row--)
            {
                bool rightToLeft = (k - 1 - row) % 2 == 0;
                for (int j = 0; j < k; j++)
                {
                    int col = rightToLeft ? k - 1 - j : j;
                    m[row, col] = arr[index];
                    index++;
                }
            }
            PrintMatrix("Матрица " + k + "x" + k + ":", m);
        }

        static void Task2()
        {
            Console.WriteLine("\n=== Задача 2: количество нечётных элементов в каждом столбце ===");
            int[,] m = RandomMatrix(4, 5, 0, 20);
            PrintMatrix("Матрица:", m);

            int[] result = new int[m.GetLength(1)];
            for (int j = 0; j < m.GetLength(1); j++)
            {
                int count = 0;
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    if (m[i, j] % 2 != 0)
                    {
                        count++;
                    }
                }
                result[j] = count;
            }
            PrintArray("Нечётных в столбцах: ", result);
        }

        static void Task3()
        {
            Console.WriteLine("\n=== Задача 3: элементы меньше среднего арифметического ===");
            int[,] m = RandomMatrix(4, 5, 0, 50);
            PrintMatrix("Матрица:", m);

            int sum = 0;
            foreach (int x in m)
            {
                sum += x;
            }
            double average = (double)sum / m.Length;
            Console.WriteLine("Среднее арифметическое: " + average);

            List<int> list = new List<int>();
            foreach (int x in m)
            {
                if (x < average)
                {
                    list.Add(x);
                }
            }
            PrintArray("Элементы меньше среднего: ", list.ToArray());
        }

        static void Task4()
        {
            Console.WriteLine("\n=== Задача 4: индексы, где A[i] и B[i] разных знаков ===");
            int[] a = RandomArray(8, -10, 10);
            int[] b = RandomArray(8, -10, 10);
            PrintArray("A: ", a);
            PrintArray("B: ", b);

            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if ((a[i] > 0 && b[i] < 0) || (a[i] < 0 && b[i] > 0))
                {
                    count++;
                }
            }
            Console.WriteLine("Количество индексов с разными знаками: " + count);
        }

        static void Task5()
        {
            Console.WriteLine("\n=== Задача 5: столбцы, где все элементы A меньше B ===");
            int[,] a = RandomMatrix(3, 6, 0, 20);
            int[,] b = RandomMatrix(3, 6, 0, 20);
            PrintMatrix("Матрица A:", a);
            PrintMatrix("Матрица B:", b);

            int count = 0;
            for (int j = 0; j < a.GetLength(1); j++)
            {
                bool allLess = true;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    if (a[i, j] >= b[i, j])
                    {
                        allLess = false;
                    }
                }
                if (allLess)
                {
                    count++;
                }
            }
            Console.WriteLine("Количество таких столбцов: " + count);
        }

        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task4();
            Task5();
            Console.ReadLine();
        }
    }
}