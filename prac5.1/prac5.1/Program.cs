using System;

namespace Sorting
{
    internal class Program
    {
        static void Print(string title, int[] arr)
        {
            Console.WriteLine(title + string.Join(" ", arr));
        }

        static void Swap(int[] a, int i, int j)
        {
            int temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }

        static void BubbleSort(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        Swap(a, j, j + 1);
                    }
                }
            }
        }

        static void InsertionSort(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int key = a[i];
                int j = i - 1;
                while (j >= 0 && a[j] > key)
                {
                    a[j + 1] = a[j];
                    j--;
                }
                a[j + 1] = key;
            }
        }

        static void SelectionSort(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[j] < a[min])
                    {
                        min = j;
                    }
                }
                Swap(a, i, min);
            }
        }

        static void ShakerSort(int[] a)
        {
            int left = 0;
            int right = a.Length - 1;
            while (left < right)
            {
                for (int i = left; i < right; i++)
                {
                    if (a[i] > a[i + 1])
                    {
                        Swap(a, i, i + 1);
                    }
                }
                right--;
                for (int i = right; i > left; i--)
                {
                    if (a[i - 1] > a[i])
                    {
                        Swap(a, i - 1, i);
                    }
                }
                left++;
            }
        }

        static void ShellSort(int[] a)
        {
            for (int gap = a.Length / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < a.Length; i++)
                {
                    int temp = a[i];
                    int j = i;
                    while (j >= gap && a[j - gap] > temp)
                    {
                        a[j] = a[j - gap];
                        j -= gap;
                    }
                    a[j] = temp;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Размер массива: ");
            int n = Convert.ToInt32(Console.ReadLine());

            Random random = new Random();
            int[] source = new int[n];
            for (int i = 0; i < n; i++)
            {
                source[i] = random.Next(0, 100);
            }
            Print("Исходный массив:   ", source);

            int[] a = (int[])source.Clone();
            BubbleSort(a);
            Print("Пузырьком:         ", a);

            a = (int[])source.Clone();
            InsertionSort(a);
            Print("Прямым включением: ", a);

            a = (int[])source.Clone();
            SelectionSort(a);
            Print("Прямым выбором:    ", a);

            a = (int[])source.Clone();
            ShakerSort(a);
            Print("Шейкерная:         ", a);

            a = (int[])source.Clone();
            ShellSort(a);
            Print("Шелла:             ", a);

            Console.ReadLine();
        }
    }
}