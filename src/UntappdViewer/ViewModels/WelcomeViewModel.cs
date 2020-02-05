﻿using System.Windows;
using Prism.Mvvm;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class WelcomeViewModel : BindableBase, IWelcomeViewModel
    {
        private IDialogService dialogService;

        public WelcomeViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void OpenFileButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
