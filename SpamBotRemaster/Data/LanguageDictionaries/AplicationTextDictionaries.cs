using System.Collections.Generic;
using SpamBotRemaster.Models;
using SpamBotRemaster.Data.Enums;
using System.Linq;

namespace SpamBotRemaster.Data.LanguageDictionaries
{
   

    public static class AplicationTextDictionaries
    {
        public static Dictionary<Language,Dictionary<string, string>> AllTextDictionaries { get; }

        public static Dictionary<string, string> RuTextDictionary { get; }
        public static Dictionary<string,string> EngTextDictionary{ get; }
        
        static AplicationTextDictionaries()
        {
           
            RuTextDictionary = new Dictionary<string, string>()
            {
                {"",null },
                {"processName","Название процесса"},
                {"dataFieldStandart","Текст или файл"},
                {"dataFieldOnDragEnter", "Сбросьте файл, которым будете спамить"},
                {"dataFieldOnDrop", "Сброшен текст или файл"},
                {"countOfMessages","Количество повторений"},
                {"delayBeforeSend","Задержка между отправкой"},
                {"delayBetweenPasteAndSend","Задержка между вставкой и отправкой"},

                {"processNameToolTip","Введите название процесса без .exe, посмотреть можно в Диспечере Задач"},
                {"dataFieldToolTip","Введите текст, которым будете спамить"},
                {"countOfMessagesToolTip","Введите сколько раз будет отправлено сообщение"},
                {"delayBeforeSendToolTip","Введите задержку между отправлением сообщений в милисекундах (рекомендуется начать с 200мс)"},
                {"delayBetweenPasteAndSendToolTip","Нужно ввести, если в программе возможность для отправления появляется не сразу после вставки текста"},
                
                {"settings","Настройки"},
                {"appLanguage","Язык приложения :"},

                {"startSpam","Начать спам"},
                {"endSpam","Закончить спам"}
            };

            EngTextDictionary = new Dictionary<string, string>()
            {
                {"",null },
                {"processName","Process name"},
                {"dataFieldStandart","Text or file"},
                {"dataFieldOnDragEnter", "Dump the file you will be spamming with"},
                {"dataFieldOnDrop", "Text or file dropped"},
                {"countOfMessages","Number of repetitions"},
                {"delayBeforeSend","Delay between sending"},
                {"delayBetweenPasteAndSend","Delay between insertion and sending"},

                {"processNameToolTip","Enter the name of the process without .exe, you can see it in the Task Manager"},
                {"dataFieldToolTip","Enter the text you will use to spam"},
                {"countOfMessagesToolTip","Enter how many times the message will be sent"},
                {"delayBeforeSendToolTip","Enter the delay between sending messages in milliseconds (it is recommended to start with 200ms)"},
                {"delayBetweenPasteAndSendToolTip","You need to enter if the opportunity for sending does not appear in the program immediately after inserting the text"},

                {"settings","Settings"},
                {"appLanguage","App language :"},

                {"startSpam","Start spam"},
                {"endSpam","End spam"}
            };

            AllTextDictionaries = new Dictionary<Language, Dictionary<string, string>>()
            {
                {Language.ru, RuTextDictionary},
                {Language.eng, EngTextDictionary}
            };
        }

        public static string GetTextByDictionaryKey(string key, Language language) => AllTextDictionaries.GetValueOrDefault(language)?.GetValueOrDefault(key) ?? "";

    }
}
