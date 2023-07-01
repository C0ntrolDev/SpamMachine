using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Input;
using SpamBotRemaster.Infrastructure.Commands;
using SpamBotRemaster.Infrastructure.Tools;
using SpamBotRemaster.Models;

namespace SpamBotRemaster.Services
{
    internal class SpamService
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private Action<SpamRequest,CancellationToken> spamAction;
        Thread spamThread;
        CancellationTokenSource cts = new();


        private LambdaCommand setCountOfSentMessagesCommand;
        private LambdaCommand exceptionCommand;



        public SpamService(LambdaCommand setCountOfSentMessagesCommand, LambdaCommand exceptionCommand)
        {
            this.setCountOfSentMessagesCommand = setCountOfSentMessagesCommand;
            this.exceptionCommand = exceptionCommand;
        }


        public void StartSpam(SpamRequest spamRequest)
        {
            if (spamThread == null || spamThread.ThreadState != System.Threading.ThreadState.Running)
            {
                if (IsAppActive(spamRequest.ProcessName))
                {
                    if (spamRequest.SpamFilesPaths != null && spamRequest.SpamFilesPaths.Count != 0)
                    {
                        spamAction = FileSpam;
                    }
                    else if (spamRequest.SpamMessageText != null && spamRequest.SpamMessageText != "")
                    {
                        spamAction = TextSpam;
                    }
                    else
                    {
                        TryCreateException("Не введен файл или текст для спама");
                        return;
                    }

                    cts = new CancellationTokenSource();

                    SetActiveApp(spamRequest.ProcessName);

                    spamThread = new Thread(() => spamAction(spamRequest,cts.Token));
                    spamThread.SetApartmentState(ApartmentState.STA);
                    spamThread.Start();
                    spamThread.IsBackground = true;
                }
                else
                {
                    TryCreateException($"Процесс не найден с именем: {spamRequest.ProcessName}");
                }
            }
            else
            {
                TryCreateException("Спам еще не закончен");
                SetActiveApp(spamRequest.ProcessName);
            }
        }
        private bool IsAppActive(string processName) => Process.GetProcessesByName(processName).Length > 0;
        private void SetActiveApp(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            SetForegroundWindow(processes[0].MainWindowHandle);
        }
        private void TryCreateException(string exceptionText)
        {
            if (exceptionCommand.CanExecute(exceptionText))
            {
                exceptionCommand.Execute(exceptionText);
            }
        }


        private void FileSpam(SpamRequest spamRequest,CancellationToken token)
        {
            var paths = new StringCollection();
            int pathIndex = 0;

            for (int i = 0; i < spamRequest.CountOfMessages; i++, pathIndex++)
            {
                if (pathIndex + 1 > spamRequest.SpamFilesPaths.Count)
                {
                    pathIndex = 0;
                }

                paths.Clear();
                paths.Add(spamRequest.SpamFilesPaths[pathIndex]);
                Clipboard.SetFileDropList(paths);

                NativeMethods.SendKey(Key.V, ModifierKeys.Control);
                Thread.Sleep(spamRequest.DelayBetweenPasteAndSend);
                NativeMethods.SendKey(Key.Enter);
                

                setCountOfSentMessagesCommand.Execute(i);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                Thread.Sleep(spamRequest.DelayBeforeSend);
            }
        }
        private void TextSpam(SpamRequest spamRequest,CancellationToken token)
        {

            for (int i = 1; i < spamRequest.CountOfMessages + 1; i++)
            {
                Clipboard.Clear();
                Clipboard.SetText(spamRequest.SpamMessageText.Replace("$", i.ToString()));

                NativeMethods.SendKey(Key.V, ModifierKeys.Control);
                NativeMethods.SendKey(Key.Enter);

                setCountOfSentMessagesCommand.Execute(i);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                Thread.Sleep(spamRequest.DelayBeforeSend);
            }
        }

        public void EndSpam()
        {
            cts.Cancel();
        }
    }
}
