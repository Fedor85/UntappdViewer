﻿using System;
using System.Reflection;
using UntappdViewer.Infrastructure;

namespace UntappdViewer.Helpers
{
    public static class CommunicationHelper
    {
        public static string GetFileStatusMessage(FileStatus fileStatus, string filePath)
        {
            string statusMessage;
            switch (fileStatus)
            {
                case FileStatus.IsEmptyPath:
                    statusMessage = Properties.Resources.IsEmptyFilePath;
                    break;
                case FileStatus.NotExists:
                    statusMessage = Properties.Resources.NotExistsFile;
                    break;
                case FileStatus.IsLocked:
                    statusMessage = Properties.Resources.IsLockedFile;
                    break;
                case FileStatus.NoSupported:
                    statusMessage = Properties.Resources.NoSupportedFile;
                    break;
                default:
                    statusMessage = Properties.Resources.OpenFile;
                    break;
            }
            return $"{statusMessage}:\n{filePath}";
        }

        public static string GetLoadingMessage(string filePath)
        {
            return $"{Properties.Resources.LoadingFrom} {filePath}";
        }

        public static string GetTitle()
        {
            return GetTitle(String.Empty);
        }

        public static string GetTitle(string userName)
        {
            return $"{Properties.Resources.AppName} {(String.IsNullOrEmpty(userName) ? String.Empty : userName)} ({Assembly.GetEntryAssembly()?.GetName().Version})";
        }
    }
}
