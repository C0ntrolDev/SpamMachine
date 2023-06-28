using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using SpamBotRemaster.Data.LanguageDictionaries;
using SpamBotRemaster.Infrastructure.Commands;
using SpamBotRemaster.Models;
using SpamBotRemaster.Services;
using SpamBotRemaster.ViewModels.Base;

namespace SpamBotRemaster.ViewModels
{
    internal class AplicationVM : ViewModel, IDropTarget
    {
        private SaveSettingsService saveSettingsService;
        private AplicationSettings aplicationSettings;

        private SpamService spamService;
        private SpamRequest spamRequest;
        

        #region Commands

        private LambdaCommand startSpamCommand;
        public LambdaCommand StartSpamCommand
        {
            get
            {
                if (startSpamCommand == null)
                {
                    startSpamCommand = new LambdaCommand(obj => spamService.StartSpam(spamRequest));
                }
                return startSpamCommand;
            }
        }


        private LambdaCommand endSpamCommand;
        public LambdaCommand EndSpamCommand
        {
            get
            {
                if (endSpamCommand == null)
                {
                    endSpamCommand = new LambdaCommand(obj => spamService.EndSpam());
                }
                return endSpamCommand;
            }
        }


        private LambdaCommand openSettingsMenuCommand;
        public LambdaCommand OpenSettingsMenuCommand
        {
            get
            {
                if (openSettingsMenuCommand == null)
                {
                    openSettingsMenuCommand = new LambdaCommand(obj => IsSettingsMenuOpen = true);
                }
                return openSettingsMenuCommand;
            }
        }

        private LambdaCommand closeSettingsMenuCommand;
        public LambdaCommand CloseSettingsMenuCommand
        {
            get
            {
                if (closeSettingsMenuCommand == null)
                {
                    closeSettingsMenuCommand = new LambdaCommand(obj => IsSettingsMenuOpen = false);
                }
                return closeSettingsMenuCommand;
            }
        }
        #endregion


        #region Properties


        private int countOfSentMessages;
        public int CountOfSentMessages
        {
            get => countOfSentMessages;
            set => Set(ref countOfSentMessages, value);
        }

        private bool isSettingsMenuOpen;
        public bool IsSettingsMenuOpen
        {
            get => isSettingsMenuOpen;
            set => Set(ref isSettingsMenuOpen, value);
            
        }


        private string startSpamButtonTextKey;
        public string StartSpamButtonText
        {
            get => AplicationTextDictionaries.GetTextByDictionaryKey(startSpamButtonTextKey, aplicationSettings.AplicationLanguage);
            set => Set(ref startSpamButtonTextKey, value);
        }

        private string endSpamButtonTextKey;
        public string EndSpamButtonText
        {
            get => AplicationTextDictionaries.GetTextByDictionaryKey(endSpamButtonTextKey, aplicationSettings.AplicationLanguage);
            set => Set(ref endSpamButtonTextKey, value);
        }

        #region ModelProperties

        public string ProcessName
        {
            get => spamRequest.ProcessName;
            set
            {
                spamRequest.ProcessName = value;
                OnPropertyChanged();
            }
        }

        public string SpamMessageText
        {
            get => spamRequest.SpamMessageText;
            set
            {
                spamRequest.SpamMessageText = value;
                OnPropertyChanged();

                if (spamRequest.SpamMessageText != null && spamRequest.SpamMessageText != "")
                {
                    SpamFilesPaths = null;
                    SpamMessageVM.PlaceholderText = "dataFieldStandart";
                }
            }

        }
        public List<string> SpamFilesPaths
        {
            get => spamRequest.SpamFilesPaths;
            set
            {
                spamRequest.SpamFilesPaths = value;
                SpamMessageText = "";
            }
        }

        public int CountOfMessages
        {
            get => spamRequest.CountOfMessages;
            set
            {
                spamRequest.CountOfMessages = value;
                OnPropertyChanged();
            }
        }

        public int DelayBeforeSend
        {
            get => spamRequest.DelayBeforeSend;
            set
            {
                spamRequest.DelayBeforeSend = value > 150 ? value : 150;
                OnPropertyChanged();
            }
        }

        public int DelayBetweenPasteAndSend
        {
            get => spamRequest.DelayBetweenPasteAndSend;
            set
            {
                spamRequest.DelayBetweenPasteAndSend = value > 150 ? value : 150;
                OnPropertyChanged();
            }
        }



        #endregion

        #region PlaceholdersTextsProperties

        public TextBoxWithPlaceholderVM ProcessNameVM { get; }
        public TextBoxWithPlaceholderVM SpamMessageVM { get; }
        public TextBoxWithPlaceholderVM CountOfMessagesVM { get; }
        public TextBoxWithPlaceholderVM DelayBeforeSendVM { get; }
        public TextBoxWithPlaceholderVM DelayBetweenPasteAndSendVM { get; }

        #endregion

        #endregion


        public AplicationVM()
        {
            spamRequest = new SpamRequest();

            spamService = new SpamService(
                setCountOfSentMessagesCommand:  new LambdaCommand(
                    SetCountOfSentMessages,
                    obj => obj != null && obj is int),

                exceptionCommand: new LambdaCommand(
                    ShowExceptionMessage,
                    obj => obj != null));


            saveSettingsService = new SaveSettingsService(@"C:\\ProgramData\SpamBotRemaster");
            aplicationSettings = saveSettingsService.ReadSettings("settings.json");

            ProcessNameVM = new TextBoxWithPlaceholderVM("processName","processNameToolTip",aplicationSettings);
            SpamMessageVM = new TextBoxWithPlaceholderVM("dataFieldStandart", "dataFieldToolTip", aplicationSettings);
            CountOfMessagesVM = new TextBoxWithPlaceholderVM("countOfMessages", "countOfMessagesToolTip", aplicationSettings);
            DelayBeforeSendVM = new TextBoxWithPlaceholderVM("delayBeforeSend", "delayBeforeSendToolTip", aplicationSettings);
            DelayBetweenPasteAndSendVM = new TextBoxWithPlaceholderVM("delayBetweenPasteAndSend", "delayBetweenPasteAndSendToolTip", aplicationSettings);

            startSpamButtonTextKey = "startSpam";
            endSpamButtonTextKey = "endSpam";
        }


        private void SetCountOfSentMessages(object parameter)
        {
            CountOfSentMessages = (int)parameter;
        }
        private void ShowExceptionMessage(object parameter)
        {
            MessageBox.Show((string)parameter);
        }


        #region Drag&Drop
        public void DragEnter(IDropInfo dropInfo)
        {
            SpamMessageVM.PlaceholderText = "dataFieldOnDragEnter";
        }
        public void DragLeave(IDropInfo dropInfo)
        {
            if (SpamMessageVM.PlaceholderText != "dataFieldOnDrop" )
            {
                SpamMessageVM.PlaceholderText = "dataFieldStandart";
            }
        }
        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
        }
        public void Drop(IDropInfo dropInfo)
        {
            List<string> paths = new List<string>();

            var dataObject = dropInfo.Data as DataObject;

            if (dataObject != null && dataObject.ContainsFileDropList())
            {
                foreach (var obj in dataObject.GetFileDropList())
                {
                    if (Directory.Exists(obj))
                        paths.AddRange(Directory.GetFiles(obj, "*.*", SearchOption.AllDirectories));
                    else
                        paths.Add(obj);
                }
            }

            SpamFilesPaths = paths;
            SpamMessageVM.PlaceholderText = "dataFieldOnDrop";
        }
        #endregion
    }
}
