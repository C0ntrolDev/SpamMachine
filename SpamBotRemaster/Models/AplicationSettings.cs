using SpamBotRemaster.Data.Enums;
using System;

namespace SpamBotRemaster.Models
{
   
    public class AplicationSettings
    {
        public event Action LanguageChanged;

        private Language language;
        public Language Language
        {
            get => language;
            set
            {
                language = value;
                LanguageChanged?.Invoke();
            }
        }

        public static AplicationSettings DeffaultSettings { get; }

        static AplicationSettings()
        {
            DeffaultSettings = new AplicationSettings()
            {
                Language = Language.eng
            };
        }
    }
}
