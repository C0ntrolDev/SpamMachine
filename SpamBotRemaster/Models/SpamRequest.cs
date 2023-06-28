using System.Collections.Generic;


namespace SpamBotRemaster.Models
{
    class SpamRequest
    {
        public string ProcessName { get; set; }
        public string SpamMessageText { get; set; }
        public List<string> SpamFilesPaths { get; set; }
        public int CountOfMessages { get; set; }
        public int DelayBeforeSend { get; set; }
        public int DelayBetweenPasteAndSend { get; set; }
    }
}

