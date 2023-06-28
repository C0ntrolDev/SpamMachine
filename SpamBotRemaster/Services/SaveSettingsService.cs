using SpamBotRemaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpamBotRemaster.Services
{
    class SaveSettingsService
    {
        private readonly string savePath;

        public SaveSettingsService(string savePath)
        {
            this.savePath = savePath;
        }

        public void SaveSettings(AplicationSettings settings, string fileName)
        {
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }

            using (FileStream fs = new FileStream(savePath + "\\" + fileName,FileMode.Create))
            { 
                JsonSerializer.Serialize(fs,settings);
            }
        }

        public AplicationSettings ReadSettings(string fileName)
        {
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }
            else
            {
                if (File.Exists(savePath + "\\" + fileName) == false)
                { 
                    SaveSettings(AplicationSettings.DeffaultSettings, fileName);
                    return AplicationSettings.DeffaultSettings;
                }
            }


            AplicationSettings settings;
            using (FileStream fs = new FileStream(savePath + "\\" + fileName, FileMode.Open))
            {
                settings = JsonSerializer.Deserialize<AplicationSettings>(fs);
            }

            return settings;
        }
    }
}
