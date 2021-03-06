﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Interactivity.InteractionRequest;

namespace UntappdViewer.Services.PopupWindowAction
{
    public class IconPopupWindowAction : Prism.Interactivity.PopupWindowAction
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(IconPopupWindowAction), new PropertyMetadata(null));

        public ImageSource Icon
        {
            get { return (ImageSource)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        protected override Window GetWindow(INotification notification)
        {
            Window defaultWindow = base.GetWindow(notification);
            IIconNotification iconNotification = notification as IIconNotification;
            if (iconNotification != null)
                defaultWindow.Icon = iconNotification.Icon;

            defaultWindow.ResizeMode = ResizeMode.NoResize;
            defaultWindow.PreviewKeyDown += PreviewKeyDown;
            return defaultWindow;
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((Window)sender).Close();               
            }
            else if (e.Key == Key.Enter)
            {
                Window window = (Window) sender;
                IConfirmation confirmation = window.DataContext as IConfirmation;
                if (confirmation != null)
                    confirmation.Confirmed = true;

                window.Close();
            }
        }
    }
}