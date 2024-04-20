using System;

namespace TeamClasses
{
    // Класс наследник EventArgs.
    public class UpdatedEventArgs: EventArgs
    {
        public DateTime UpdateDateTime { get;  }
        public UpdatedEventArgs(DateTime updateDateTime)
        {
            UpdateDateTime = updateDateTime;
        }
        // Пустой конструктор.
        public UpdatedEventArgs()
        {
            UpdateDateTime = DateTime.MinValue;
        }
    }
}