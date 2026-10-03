using System;

namespace Calculator
{
    internal class Program
    {
        static int operationsCount = 0;

        static double ReadNumber()
        {
            Console.Write("Введите число: ");
            return Convert.ToDouble(Console.ReadLine());
        }

        static void Sum()
        {
            double a = ReadNumber();
            double b = ReadNumber();
            Console.WriteLine("Сумма = " + (a + b));
        }

        static void Subtract(double a, double b)
        {
            Console.WriteLine("Разность = " + (a - b));
        }

        static double Multiply(double a, double b)
        {
            return a * b;
        }

        static void Divide(double a, double b, out double result)
        {
            result = a / b;
        }

        static void Increment(ref int number)
        {
            number = number + 1;
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Сложение");
                Console.WriteLine("2 - Вычитание");
                Console.WriteLine("3 - Умножение");
                Console.WriteLine("4 - Деление");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    break;
                }

                switch (choice)
                {
                    case "1":
                        Sum();
                        Increment(ref operationsCount);
                        break;
                    case "2":
                        {
                            double a = ReadNumber();
                            double b = ReadNumber();
                            Subtract(a, b);
                            Increment(ref operationsCount);
                            break;
                        }
                    case "3":
                        {
                            double a = ReadNumber();
                            double b = ReadNumber();
                            double result = Multiply(a, b);
                            Console.WriteLine("Произведение = " + result);
                            Increment(ref operationsCount);
                            break;
                        }
                    case "4":
                        {
                            double a = ReadNumber();
                            double b = ReadNumber();
                            if (b == 0)
                            {
                                Console.WriteLine("Ошибка: деление на ноль");
                                break;
                            }
                            double result;
                            Divide(a, b, out result);
                            Console.WriteLine("Частное = " + result);
                            Increment(ref operationsCount);
                            break;
                        }
                    default:
                        Console.WriteLine("Нет такого пункта");
                        break;
                }
            }

            Console.WriteLine("Выполнено операций: " + operationsCount);
            Console.ReadLine();
        }
    }
}