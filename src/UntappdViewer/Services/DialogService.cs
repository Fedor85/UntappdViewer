﻿using System;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Services
{
    public class DialogService : IDialogService
    {
        public MessageBoxResult Ask(string caption, string message)
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OKCancel);
        }

        public string OpenFile(params string[] extensions)
         {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = GetFilter(extensions);
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }

        private string GetFilter(params string[] extensions)
        {
            if (extensions.Length == 0)
                return String.Empty;

            StringBuilder filter = new StringBuilder();
            foreach (string extension in extensions)
                filter.AppendFormat("(*.{0})|*.{0}|", extension);

            return filter.Remove(filter.Length - 1, 1).ToString();
        }
    }
}