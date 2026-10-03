using System;
using System.Collections.Generic;

namespace Coworking
{
    struct Workplace
    {
        public int Number;
        public string Type;
        public double PricePerHour;
        public bool IsBusy;

        public Workplace(int number, string type, double pricePerHour, bool isBusy)
        {
            Number = number;
            Type = type;
            PricePerHour = pricePerHour;
            IsBusy = isBusy;
        }
    }

    class CoworkingManager
    {
        private List<Workplace> places = new List<Workplace>();

        public CoworkingManager()
        {
            places.Add(new Workplace(1, "обычное", 200, false));
            places.Add(new Workplace(2, "обычное", 200, true));
            places.Add(new Workplace(3, "обычное", 250, false));
            places.Add(new Workplace(4, "переговорное", 800, false));
            places.Add(new Workplace(5, "переговорное", 1000, true));
            places.Add(new Workplace(6, "переговорное", 900, false));
        }

        public void ShowAll()
        {
            Console.WriteLine("Номер | Тип          | Цена/час | Статус");
            foreach (Workplace p in places)
            {
                string status = p.IsBusy ? "занято" : "свободно";
                Console.WriteLine(p.Number + "     | " + p.Type.PadRight(12) + " | "
                    + p.PricePerHour.ToString().PadRight(8) + " | " + status);
            }
        }

        public void PotentialRevenue(int hours)
        {
            double all = 0;
            double busy = 0;
            foreach (Workplace p in places)
            {
                all += p.PricePerHour * hours;
                if (p.IsBusy)
                {
                    busy += p.PricePerHour * hours;
                }
            }
            Console.WriteLine("Выручка за " + hours + " ч, если заняты все места: " + all + " руб.");
            Console.WriteLine("Выручка за " + hours + " ч по занятым сейчас местам: " + busy + " руб.");
        }

        public void ShowFreeMeetingRooms()
        {
            int count = 0;
            foreach (Workplace p in places)
            {
                if (p.Type == "переговорное" && !p.IsBusy)
                {
                    Console.WriteLine("Переговорная №" + p.Number + ", " + p.PricePerHour + " руб./час");
                    count++;
                }
            }
            if (count == 0)
            {
                Console.WriteLine("Свободных переговорных нет");
            }
        }

        public void ChangeStatus(int number)
        {
            for (int i = 0; i < places.Count; i++)
            {
                if (places[i].Number == number)
                {
                    Workplace p = places[i];
                    p.IsBusy = !p.IsBusy;
                    places[i] = p;
                    Console.WriteLine("Место №" + number + " теперь " + (p.IsBusy ? "занято" : "свободно"));
                    return;
                }
            }
            Console.WriteLine("Нет места с таким номером");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            CoworkingManager coworking = new CoworkingManager();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Показать все места");
                Console.WriteLine("2 - Рассчитать потенциальную выручку");
                Console.WriteLine("3 - Свободные переговорные");
                Console.WriteLine("4 - Изменить статус места");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        coworking.ShowAll();
                        break;
                    case "2":
                        Console.Write("Количество часов: ");
                        coworking.PotentialRevenue(Convert.ToInt32(Console.ReadLine()));
                        break;
                    case "3":
                        coworking.ShowFreeMeetingRooms();
                        break;
                    case "4":
                        Console.Write("Номер места: ");
                        coworking.ChangeStatus(Convert.ToInt32(Console.ReadLine()));
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Нет такого пункта");
                        break;
                }
            }
        }
    }
}