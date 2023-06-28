using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SpamBotRemaster.ViewModels.Base
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field,T value, [CallerMemberName]string propertyName ="")
        {
            if (Equals(field,value))
            {
                return false;
            }

            

            field = value;
            OnPropertyChanged(propertyName);
            return true;

             
        }
    }
}
