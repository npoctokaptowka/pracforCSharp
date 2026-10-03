using System;
using System.IO;

namespace Strings
{
    internal class Program
    {
        static string ReadInput()
        {
            Console.Write("Имя входного файла: ");
            string path = Console.ReadLine().Trim().Trim('"');
            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                return "";
            }
            Console.WriteLine("Прочитана строка: " + lines[0]);
            return lines[0];
        }

        static void WriteOutput(string result)
        {
            Console.Write("Имя выходного файла: ");
            string path = Console.ReadLine().Trim().Trim('"');
            File.WriteAllText(path, result);
            Console.WriteLine("Результат: " + result);
            Console.WriteLine("Успешно записано в файл " + Path.GetFullPath(path));
        }

        static string CountWords(string s)
        {
            string[] words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return "Количество слов: " + words.Length;
        }

        static string Reverse(string s)
        {
            char[] chars = s.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        static string CountVowels(string s)
        {
            string vowels = "аеёиоуыэюяaeiouy";
            int count = 0;
            foreach (char c in s.ToLower())
            {
                if (vowels.Contains(c.ToString()))
                {
                    count++;
                }
            }
            return "Количество гласных: " + count;
        }

        static string Capitalize(string s)
        {
            string[] words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
            return string.Join(" ", words);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Подсчитать количество слов");
                Console.WriteLine("2 - Перевернуть строку");
                Console.WriteLine("3 - Подсчитать количество гласных");
                Console.WriteLine("4 - Каждое слово с заглавной буквы");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    break;
                }
                if (choice != "1" && choice != "2" && choice != "3" && choice != "4")
                {
                    Console.WriteLine("Нет такого пункта");
                    continue;
                }

                try
                {
                    string text = ReadInput();
                    string result = "";
                    switch (choice)
                    {
                        case "1": result = CountWords(text); break;
                        case "2": result = Reverse(text); break;
                        case "3": result = CountVowels(text); break;
                        case "4": result = Capitalize(text); break;
                    }
                    WriteOutput(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: " + e.Message);
                }
            }
        }
    }
}