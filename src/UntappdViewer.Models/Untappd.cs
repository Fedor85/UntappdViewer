﻿using System;
using System.Collections.Generic;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Untappd
    {
        public CheckinsContainer CheckinsContainer { get; private set; }

        ///// <summary>
        /// Чекины
        /// </summary>
        public List<Checkin> Checkins { get { return CheckinsContainer.Checkins; } }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Дата создания проекта
        /// </summary>
        public DateTime CreatedDate { get;}

        private bool isСhanges { get; set; }

        public Untappd(string userName)
        {
            CheckinsContainer = new CheckinsContainer();
            UserName = userName;
            CreatedDate = DateTime.Now;
            isСhanges = false;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
            isСhanges = true;
        }

        public bool IsСhanges()
        {
            return isСhanges || CheckinsContainer.IsСhanges;
        }

        public void ResetСhanges()
        {
            isСhanges = false;
            CheckinsContainer.IsСhanges = false;
        }


        public override string ToString()
        {
            return $"UserName:{UserName}/CreatedDate:{CreatedDate}/CheckinsCount:{Checkins.Count}";
        }
    }
}