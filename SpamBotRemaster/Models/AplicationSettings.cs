using SpamBotRemaster.Data.Enums;

namespace SpamBotRemaster.Models
{
   
    public class AplicationSettings
    {
        public Language AplicationLanguage{ get; set; }

        public static AplicationSettings DeffaultSettings { get; }

        static AplicationSettings()
        {
            DeffaultSettings = new AplicationSettings()
            {
                AplicationLanguage = Language.eng
            };
        }
    }
}
