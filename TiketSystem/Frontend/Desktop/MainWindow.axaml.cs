using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using Avalonia.Logging;
using Avalonia.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using TicketSystem.Backend.Models;

namespace Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

    }
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        DataContext = mainViewModel;
        mainViewModel.LoadTaskList();
    }
}