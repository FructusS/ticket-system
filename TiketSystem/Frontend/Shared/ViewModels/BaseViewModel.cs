﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
        }
    }
}