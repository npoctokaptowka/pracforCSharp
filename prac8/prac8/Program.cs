using System;

namespace CoworkingApp
{
    class Coworking
    {
        private int[] numbers = new int[100];
        private string[] types = new string[100];
        private double[] prices = new double[100];
        private bool[] busy = new bool[100];
        private int count = 0;

        public Coworking()
        {
            Add(1, "обычное", 200, false);
            Add(2, "обычное", 250, true);
            Add(3, "переговорное", 800, false);
            Add(4, "переговорное", 1000, true);
        }

        private void Add(int number, string type, double price, bool isBusy)
        {
            numbers[count] = number;
            types[count] = type;
            prices[count] = price;
            busy[count] = isBusy;
            count++;
        }

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                int value;
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine("Введите целое число");
            }
        }

        private double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                double value;
                if (double.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine("Введите число");
            }
        }

        private int FindIndex(int number)
        {
            for (int i = 0; i < count; i++)
            {
                if (numbers[i] == number)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddPlace()
        {
            if (count == numbers.Length)
            {
                Console.WriteLine("Нет места для новых записей");
                return;
            }
            int number = ReadInt("Номер места: ");
            if (FindIndex(number) != -1)
            {
                Console.WriteLine("Место с таким номером уже есть");
                return;
            }
            int typeChoice = ReadInt("Тип (1 - обычное, 2 - переговорное): ");
            string type = typeChoice == 2 ? "переговорное" : "обычное";
            double price = ReadDouble("Стоимость часа: ");
            Add(number, type, price, false);
            Console.WriteLine("Место добавлено");
        }

        public void ShowAll()
        {
            if (count == 0)
            {
                Console.WriteLine("Мест нет");
                return;
            }
            for (int i = 0; i < count; i++)
            {
                string status = busy[i] ? "занято" : "свободно";
                Console.WriteLine("№" + numbers[i] + " | " + types[i] + " | " + prices[i] + " руб./час | " + status);
            }
        }

        public void PotentialRevenue()
        {
            int hours = ReadInt("Количество часов: ");
            double all = 0;
            double current = 0;
            for (int i = 0; i < count; i++)
            {
                all += prices[i] * hours;
                if (busy[i])
                {
                    current += prices[i] * hours;
                }
            }
            Console.WriteLine("Если заняты все места: " + all + " руб.");
            Console.WriteLine("По занятым сейчас местам: " + current + " руб.");
        }

        public void ShowFreeMeetingRooms()
        {
            int found = 0;
            for (int i = 0; i < count; i++)
            {
                if (types[i] == "переговорное" && !busy[i])
                {
                    Console.WriteLine("Переговорная №" + numbers[i] + ", " + prices[i] + " руб./час");
                    found++;
                }
            }
            if (found == 0)
            {
                Console.WriteLine("Свободных переговорных нет");
            }
        }

        public void ChangeStatus()
        {
            int index = FindIndex(ReadInt("Номер места: "));
            if (index == -1)
            {
                Console.WriteLine("Нет места с таким номером");
                return;
            }
            busy[index] = !busy[index];
            Console.WriteLine("Место №" + numbers[index] + " теперь " + (busy[index] ? "занято" : "свободно"));
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Coworking coworking = new Coworking();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Добавить место");
                Console.WriteLine("2 - Показать все места");
                Console.WriteLine("3 - Рассчитать потенциальную выручку");
                Console.WriteLine("4 - Свободные переговорные");
                Console.WriteLine("5 - Изменить статус места");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": coworking.AddPlace(); break;
                    case "2": coworking.ShowAll(); break;
                    case "3": coworking.PotentialRevenue(); break;
                    case "4": coworking.ShowFreeMeetingRooms(); break;
                    case "5": coworking.ChangeStatus(); break;
                    case "0": return;
                    default: Console.WriteLine("Нет такого пункта"); break;
                }
            }
        }
    }
}