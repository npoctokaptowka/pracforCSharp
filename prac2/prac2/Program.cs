using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Задание 1: среднее арифметическое");
        Console.Write("Начало диапазона: ");
        int start = int.Parse(Console.ReadLine());
        Console.Write("Конец диапазона: ");
        int end = int.Parse(Console.ReadLine());

        int step = start <= end ? 1 : -1;
        int count = 0;
        int sum = 0;

        Console.WriteLine("Числа диапазона:");
        for (int i = start; i != end + step; i += step)
        {
            Console.Write(i + " ");
            sum += i;
            count++;
        }
        Console.WriteLine();
        Console.WriteLine("Количество чисел: " + count);
        Console.WriteLine("Среднее арифметическое: " + (double)sum / count);

        Console.WriteLine();
        Console.WriteLine("Задание 2: S = 5 + 5/2 + 5/3 + ... + 5/N");
        Console.Write("Введите N: ");
        int n = int.Parse(Console.ReadLine());
        if (n < 1)
        {
            Console.WriteLine("N должно быть не меньше 1");
            return;
        }

        double s = 0;
        for (int i = 1; i <= n; i++)
        {
            s += 5.0 / i;
        }
        Console.WriteLine("S = " + s);
    }
}