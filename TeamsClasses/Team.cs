using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeamClasses
{
    public class Team
    {
        // Реализуем событие.
        public event EventHandler<UpdatedEventArgs> Updated;
        [JsonConstructor]
        // Конструктор класса.
        public Team(string teamId, string teamName, List<Player> players)
        {
            TeamId = teamId;
            TeamName = teamName ;
            Players = players;
        }
        
        // Конструктор без параметров.
        public Team()
        {
            TeamId = "";
            TeamName = "" ;
            Players = new List<Player>();
        }
        // При изменении команды вызываем событие сохраяняя время вызова.
        public void UpdateTeam(string message)
        {
            Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
        }
        [JsonPropertyName("teamId")]
        // Для кажого поля (свойства это и есть поля) делаем возможность для чтения и закрываем возможность записи.
        public string TeamId{ get; private set; }
        
        [JsonPropertyName("teamName")]
        public string TeamName{ get; private set; }
        
        // Композиция является частным случаем ассоциации. В данном случае поле players содержит список объектов класса Player;
        [JsonPropertyName("players")]
        public List<Player> Players{ get; private set; }
        // Сериализация данных.
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }); // Последний параметр используем для отступов в итоговом файле
        }
        // Делаем возможнотсть через метод менять имя команды.
        public void ChangeTeamName(string newTeamName)
        {
            TeamName = newTeamName;
        }
    }
}