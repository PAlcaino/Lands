﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lands2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backinField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backinField, value))
            {
                return;
            }
            backinField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
