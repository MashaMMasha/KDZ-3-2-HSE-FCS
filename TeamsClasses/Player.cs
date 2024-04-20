using System;
using System.Data.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeamClasses
{
    public class Player
    {
        // Создаем событие.
        public event EventHandler<UpdatedEventArgs> Updated;
        // Конструктор с параметрами.
        [JsonConstructor]
        public Player(string playerId, string playerName, string position, int jerseyNumber, bool isCaptain)
        {
            PlayerId = playerId;
            PlayerName = playerName ;
            Position = position;
            JerseyNumber = jerseyNumber;
            IsCaptain = isCaptain;
        }
        // Конструктор без параметров.
        public Player()
        {
            PlayerId = "";
            PlayerName = "";
            Position = "";
            JerseyNumber = 0;
            IsCaptain = false;
        }
        // Делаем все поля(они обернуты в свойства) закрытыми для записи и откртыми для чтения.
        [JsonPropertyName("playerId")]
        public string PlayerId{ get; private set; }

        [JsonPropertyName("playerName")]
        public string PlayerName { get; private set; }

        [JsonPropertyName("position")]
        public string Position { get; private set; }

        [JsonPropertyName("jerseyNumber")]
        public int JerseyNumber { get; private set; }
    
        [JsonPropertyName("isCaptain")]
        public bool IsCaptain { get; private set; }
        // Сериализуем объекты.
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
        // Методы для изменения данных в командах.
        public void ChangePlayerName(string newPlayerName)
        {
            PlayerName = newPlayerName;
        }
        public void ChangePosition(string newPosition)
        {
            Position = newPosition;
        }
        public void ChangeJerseyNumber(string newJerseyNumber)
        {
            // Делаем проверку на корректность заданных данных
            int correctJerseyNumber = JerseyNumber;
            if (!int.TryParse(newJerseyNumber, out correctJerseyNumber))
            {
                Console.WriteLine("Вы ввели некорректные данные!");
            }
            JerseyNumber = correctJerseyNumber;
        }
        public void ChangeCaptainStatus()
        {
            if (IsCaptain == true)
            {
                IsCaptain = false;
            }
            else
            {
                IsCaptain = true;
            }
            // Вызываем событие.
            Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
        }
        
    }
}