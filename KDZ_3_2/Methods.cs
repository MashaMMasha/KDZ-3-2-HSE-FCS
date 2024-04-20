using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TeamClasses;

namespace KDZ_3_2
{
    public class Methods
    {
        // Храним поле с путем текущего файла.
        public static string fPath = "";
        // Метод для считывания 
        public static List<Team> ReadDataFromJsonFile()
        {
            List<Team>? teams;
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите абсолютный путь до файла");
                    string? fPath = Console.ReadLine();
                    // Обновляем поле пути к файлу.
                    Methods.fPath = $"{fPath}";
                    // Проверяем на существование файла.
                    if (File.Exists(fPath))
                    {
                        string jsonData = File.ReadAllText(fPath);
                        // Десериализуем JSON в список команд.
                        teams = JsonSerializer.Deserialize<List<Team>>(jsonData);
                        foreach (var team in teams)
                        {
                            // Проверм что в файле нет пустых полей иначе сообщаем о неверном формате.
                            if (team.TeamId == null || team.TeamName == null || team.Players == null ||
                                team.Players.Any(
                                    player =>
                                        player.PlayerId == null || player.PlayerName == null ||
                                        player.Position == null ||
                                        player.JerseyNumber == null || player.IsCaptain == null))
                            {
                                throw new NullReferenceException("Неверный формат данных в JSON файле.\n" +
                                                                 "Введите корректный путь.");
                            }
                        }

                        return teams;

                    }
                    else
                    {
                        Console.WriteLine("Такого файла не существует!");
                    }
                }
                // Обрабатываем возможные исключения.
                catch (System.Text.Json.JsonException)
                {
                    Console.WriteLine("Неверный данные в файле!");
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при работе с файлом! Проверьте кореектность входных данных");
                }
            }
        }

        // Метод для считывания числа от 1 до заданной границы.
        public static int ReadNumber(int k)
        {
            int n;
            Console.WriteLine($"Введите число от 1 до {k}");
            // Проверем что то, что пользователь вводит в консоль - число, в необходимых пределах.
            while (!int.TryParse(Console.ReadLine(), out n) || n > k || n < 1)
            {
                Console.WriteLine($"Вы ввели неверные данные!!! Введите число от 1 до {k}");
            }

            return n;
        }
        // Перечисление для выбораполя класса.
        public enum TeamField
        {
            TeamId = 1,
            TeamName,
            Playres,
            None
        }

        // Общение с пользователем перед вызовом сортировок.
        public static List<Team> Sorting(List<Team> teamsData)
        {
            List<Team> sortedTeam = new List<Team>();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("По какому полю вы хотите произвести сортировку? Выберете нужный вариант. \n" +
                              "1. teamId \n" +
                              "2. teamName");
            TeamField sortField = (TeamField)ReadNumber(2);
            Console.WriteLine("Как вы хотите произвести сортировку? Выберете нужный вариант. \n " +
                              "1. По возрастанию \n " +
                              "2. По убыванию");
            int sortingParametr = ReadNumber(2);
            // Вызываем нужный метод для сортировки в зависимости от решения пользователя.
            switch (sortingParametr)
            {
                case 1:
                    return SortingByAscending(teamsData, sortField);
                case 2:
                    return SortingByDescending(teamsData, sortField);
            }
            return new List<Team>();
        }
        // Метод сортировки по возрастанию.
        public static List<Team> SortingByAscending(List<Team> TeamData, TeamField sortingField)
        {
            List<Team> sortedData = new List<Team>();
            // Сортируем по полю выбранному пользователем.
            switch (sortingField)
            {
                case TeamField.TeamId:
                    sortedData = TeamData.OrderBy(v => v.TeamId).ToList();
                    WriteToConsole(sortedData, sortingField,1);
                    return sortedData;
                case TeamField.TeamName:
                    sortedData = TeamData.OrderBy(v => v.TeamName).ToList();
                    WriteToConsole(sortedData, sortingField,1);
                    return sortedData;
                default:
                    Console.WriteLine("Такого поля нет");
                    return TeamData;
            }
        }
        // Дополнительный метод для сортироки по убыванию для расширения функционала программы.
        public static List<Team> SortingByDescending(List<Team> TeamData, TeamField sortingField)
        {
            List<Team> sortedData = new List<Team>();
            switch (sortingField)
            {
                case TeamField.TeamId:
                    sortedData = TeamData.OrderByDescending(v => v.TeamId).ToList();
                    WriteToConsole(sortedData, sortingField, 1);
                    return sortedData;
                case TeamField.TeamName:
                    sortedData = TeamData.OrderByDescending(v => v.TeamName).ToList();
                    WriteToConsole(sortedData, sortingField, 2);
                    return sortedData;
                default:
                    Console.WriteLine("Такого поля нет");
                    return TeamData;
            }
        }
        // Метод для звывода данных в консоль.
        public static void WriteToConsole(List<Team> teams, TeamField field, int sortingParam)
        {
            // Метод очень длинный потому что я хотела красивый вывод и посоянно меняла цвета чтобы все выглядело красиво.
            foreach (var team in teams)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                int n = Console.WindowWidth - 1;
                if (sortingParam == 0)
                {
                    Console.WriteLine(new string('-', n));
                }
                else if (sortingParam == 1)
                {
                    // Для удобства пользователя выводим как мы сортировали и по какому параметру. 
                    string s = $"sorting by ascending by {field}";
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(new string('-', (n - s.Length)/2));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(s);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(new string('-', (n - s.Length)/2));
                }
                else if (sortingParam == 2)
                {
                    string s = $"sorting by descending by {field}";
                    Console.Write(new string('-', (n - s.Length)/2));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(s);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(new string('-', (n - s.Length)/2));
                }
                // Строим таблицу для красивого вывода.
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"╔══════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.Write("║");
                Console.ForegroundColor = ConsoleColor.White;
                // Добавляем в шапку таблицы данные о команде.
                string s1 = "TeamId: ";
                s1 += team.TeamId;
                s1 += " TeamName: ";
                s1 += team.TeamName;
                int n1 = (114 - s1.Length) / 2;
                Console.Write(new string(' ', n1));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("TeamId: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(team.TeamId);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" TeamName: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(team.TeamName);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(new string(' ', 114 - n1 - s1.Length));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("║");
                Console.WriteLine($"╠══════════════════════════════════════╦══════════════════════╦═══════════════════╦════════════════╦═══════════════╣\n" +
                                  $"║               playerId               ║      playerName      ║      position     ║  jerseyNumber  ║     capitan   ║");
                foreach (var player in team.Players)
                {
                    // Строками таблицы делаем данные о каждом игроке.
                    Console.WriteLine($"╠══════════════════════════════════════╬══════════════════════╬═══════════════════╬════════════════╬═══════════════╣");
                    Console.Write($"\u2551 ");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (player.PlayerId.Length > 36)
                    {
                        Console.Write(player.PlayerId.Substring(0,33)+"...");
                    }
                    else
                    {
                        Console.Write(string.Format("{0,36}",player.PlayerId));
                    }

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" \u2551 ");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (player.PlayerName.Length > 20)
                    {
                        Console.Write(player.PlayerName.Substring(0,17)+"...");
                    }
                    else
                    {
                        Console.Write(string.Format("{0,20}",player.PlayerName));
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" \u2551 ");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (player.Position.Length > 17)
                    {
                        Console.Write(player.Position.Substring(0,17)+"...");
                    }
                    else
                    {
                        Console.Write(string.Format("{0,17}",player.Position));
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" \u2551 ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(string.Format("{0,14}",player.JerseyNumber));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" \u2551 ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(string.Format("{0,14}",player.IsCaptain));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\u2551");
                    
                }
                Console.WriteLine($"╚══════════════════════════════════════╩══════════════════════╩═══════════════════╩════════════════╩═══════════════╝");
                // Надеюсь вывод хоррший я писала 2 часа :(
            }
        }
        // Метод позволяет пользователю изменить данные.
        public static void ChangeTeamsData(List<Team> teamsData)
        {
            // Выводим кратко все данные.
            Console.WriteLine("У какой команды вы хотите изменить данные?");
            for (int i = 0; i < teamsData.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{i+1}. ID команды: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(teamsData[i].TeamId);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($" Название команды:  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(teamsData[i].TeamName);
            }

            int teamChoice = ReadNumber(teamsData.Count);
            Console.WriteLine("Что вы хотите поменять?\n1. Название команды\n2. Данные об игроках");
            int choice = ReadNumber(2);
            if (choice == 1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Введите новое название команды: ");
                Console.ForegroundColor = ConsoleColor.White;
                teamsData[teamChoice-1].ChangeTeamName(Console.ReadLine());
            }
            else
            {
                // Если пользователь хочет изменить что-то во вложенном объекте выводим данные кратко обо всех игроках.
                Console.WriteLine("У какого игрока вы хотите изменить данные?");
                for (int i = 0; i < teamsData[teamChoice-1].Players.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{i + 1}. ID игрока: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(teamsData[teamChoice-1].Players[i].PlayerId);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($" Имя игрока:  ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(teamsData[teamChoice-1].Players[i].PlayerName);
                }

                int playerChoice = ReadNumber(teamsData[teamChoice-1].Players.Count);
                Console.WriteLine("Что вы хотите поменять?\n1. Имя игрока\n" +
                                  "2. Игровую позицию\n3. Номер игроках\n4. Статус капитана");
                int fieldChoice = ReadNumber(4);
                switch (fieldChoice)
                {
                    // Считываем на что пользователь хочет заменить данные и вызываем соответствующие методы.
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Введите новое имя игрока: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        teamsData[teamChoice-1].Players[playerChoice-1].ChangePlayerName(Console.ReadLine());
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Введите новую позицию игрока: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        teamsData[teamChoice-1].Players[playerChoice-1].ChangePosition(Console.ReadLine());
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Введите новый номер игрока (целое число!): ");
                        Console.ForegroundColor = ConsoleColor.White;
                        teamsData[teamChoice-1].Players[playerChoice-1].ChangeJerseyNumber(Console.ReadLine());
                        break;
                    case 4:
                        // Тут мы просто меняем статус капитана на противоположный так как есть всего два варианта и иначе длеать нелогично.
                        if (teamsData[teamChoice - 1].Players[playerChoice - 1].IsCaptain == false)
                        {
                            DemoteTheCaptain(teamsData[teamChoice-1].Players);
                            Console.WriteLine($"Новый капитан - {teamsData[teamChoice-1].Players[playerChoice-1].PlayerName}");
                        }
                        else
                        {
                            Console.WriteLine($"{teamsData[teamChoice-1].Players[playerChoice].PlayerName} больше не капитан.");
                            teamsData[teamChoice-1].Players[ChooseNewCaptain(teamsData[teamChoice-1].Players, playerChoice)].ChangeCaptainStatus();
                        }
                        // Метод вызывает событие после изменения статуса капитана одного из игроков.
                        teamsData[teamChoice-1].UpdateTeam("");
                        teamsData[teamChoice-1].Players[playerChoice-1].ChangeCaptainStatus();
                        break;
                }
            }
        }
        // Метод разжалует прошлого капитана.
        public static void DemoteTheCaptain(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (player.IsCaptain == true)
                {
                    Console.WriteLine($"{player.PlayerName} больше не капитан.");
                    player.ChangeCaptainStatus();
                }
            }
        }
        // Выбираем новго капитана с учетом того что игрок который им уже является не может сохранить свою позици.ю
        public static int ChooseNewCaptain(List<Player> players, int captain)
        {
            Random rnd = new Random();
            int newCapitan = rnd.Next(players.Count);
            while (newCapitan == captain)
            {
                newCapitan = rnd.Next(players.Count);
            }
            Console.WriteLine($"Новый капитан - {players[newCapitan].PlayerName}");
            return newCapitan;
        }
    }
}