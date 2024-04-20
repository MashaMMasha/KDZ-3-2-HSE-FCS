using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TeamClasses
{
    public class AutoSaver
    {
        public DateTime _lastEventTime;
        private string _filePath;
        private List<Team> _teams;
        // Коструктор без параметров.
        public AutoSaver()
        {
            _lastEventTime = DateTime.MinValue;
            _filePath = "";
            _teams = new List<Team>();

        }
        // Конструктор с параметрами.
        public AutoSaver(string path, List<Team> teams)
        {
            _filePath = path;
            _teams = teams;
            _lastEventTime = DateTime.MinValue;
        }
        
        public DateTime LastEventTime { get; set; }
        public string FilePath { get; set; }
        public List<Team> Teams { get; set; }
        // Подписывем все команды на событие.
        public void SubscribeToEvents(List<Team> teams)
        {
            Teams = teams;
            foreach (var Team in teams)
            {
                Team.Updated += TeamUpdatedHandler;
            }
        }
        // Реализуем автосохранение.
        private void TeamUpdatedHandler(object sender, UpdatedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            // Проверяем что разница во времени между событими не более 15 секунд.
            if ((currentTime - LastEventTime).TotalSeconds <= 15)
            {
                // Имя файла формируем из исходного, сохранняя путь и добавляя в название _tmp.
                string fileName = FilePath.Substring(0, FilePath.Length - 5)+"_tmp.json";
                SaveTeamsToJson(Teams, fileName);
            }
            // Обновляем время последнего события.
            LastEventTime = currentTime;
        }

        public static void SaveTeamsToJson(List<Team> teams, string fileName)
        {
            // Преобразуем каждой команды в JSON и добавление в список.
            List<string> TeamJsonList = new List<string>();
            foreach (var Team in teams)
            {
                // Используем метод прописанных в классахю
                TeamJsonList.Add(Team.ToJson());
            }

            // Объединяем JSON всех команд в одну строку для записи в файл.
            string jsonArray = "[" + string.Join(",\n", TeamJsonList) + "]";

            // Записываем JSON-строки в файл.
            File.WriteAllText(fileName, jsonArray);

            Console.WriteLine($"Данные успешно сохранены в файле: {fileName}");

        }
    }
}