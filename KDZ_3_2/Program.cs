using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TeamClasses;

namespace KDZ_3_2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Устанавливаем цвет текста и консоли.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            // Считываем данные из файла для начала работы программы.
            List<Team> data = Methods.ReadDataFromJsonFile();
            // Создаем экземпляр класса AutoSaver.
            AutoSaver autoSaver = new AutoSaver();
            // Сохраняем в классе путь файла с которым работаем.
            autoSaver.FilePath = Methods.fPath;
            // Подписываем все команды на AutoSaver.
            autoSaver.SubscribeToEvents(data);
            do
            {
                try
                {
                    // Предалагем пользователю меню.
                    Console.WriteLine(
                        "Что вы хотите сделать?\n1. Передать путь к новому файлу и работать с ним.\n2. Отсортировать данные по одному из полей.\n3. Отредактировать данные.");
                    int n = Methods.ReadNumber(3);
                    // Обрабатываем все варианты из меню.
                    switch (n)
                    {
                        case 1:
                            data = Methods.ReadDataFromJsonFile();
                            // Если пользователь работает с новым файлом создаем заново AutoSaver и подписываем на него новые команды.
                            autoSaver = new AutoSaver();
                            autoSaver.FilePath = Methods.fPath;
                            autoSaver.SubscribeToEvents(data);
                            break;
                        case 2:
                            data = Methods.Sorting(data);
                            break;
                        case 3:
                            Methods.ChangeTeamsData(data);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Произошла ошибка при работе с файлом. Повторите попытку"); // Обрабатываем исключения.
                }
                Console.WriteLine("Для завершения работы программы нажмите escape, чтобы продолжить - enter.");
                    
            } while (Console.ReadKey().Key != ConsoleKey.Escape); // Делаем цикл повторения решения.
        }
    }
}