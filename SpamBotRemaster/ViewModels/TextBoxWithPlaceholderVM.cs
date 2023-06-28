﻿using GongSolutions.Wpf.DragDrop;
using SpamBotRemaster.Data.LanguageDictionaries;
using SpamBotRemaster.Models;
using SpamBotRemaster.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpamBotRemaster.ViewModels
{
    public class TextBoxWithPlaceholderVM : ViewModel
    {
        private AplicationSettings aplicationSettings;


        private string placeholderTextKey;
        public string PlaceholderText
        {
            get => AplicationTextDictionaries.GetTextByDictionaryKey(placeholderTextKey, aplicationSettings.AplicationLanguage);
            set => Set(ref placeholderTextKey, value);
        }

        private string toolTipTextKey;
        public string ToolTipText
        {
            get => AplicationTextDictionaries.GetTextByDictionaryKey(toolTipTextKey, aplicationSettings.AplicationLanguage);
            set => Set(ref toolTipTextKey, value);
        }


        public TextBoxWithPlaceholderVM(string placeholderTextKey = "", string toolTipTextKey = "", AplicationSettings aplicationSettings = null)
        {
            this.aplicationSettings ??= AplicationSettings.DeffaultSettings;
            PlaceholderText = placeholderTextKey;
            ToolTipText = toolTipTextKey;
        }
    }
}
