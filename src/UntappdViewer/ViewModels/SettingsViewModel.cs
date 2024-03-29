﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class SettingsViewModel: LoadingBaseModel
    {
        private ISettingService settingService;

        private IWebApiClient webApiClient;

        private IModuleManager moduleManager;

        private IInteractionRequestService interactionRequestService;

        private string accessToken;

        private bool? isValidAccessToken;

        private bool isShowAccessToken;

        private string credentialsProviderBing;

        private bool isShowCredentialsProviderBing;

        public string AccessToken
        {
            get { return accessToken; }
            set { SetProperty(ref accessToken, value); }
        }

        public bool? IsValidAccessToken
        {
            get { return isValidAccessToken; }
            set { SetProperty(ref isValidAccessToken, value); }
        }

        public bool IsShowAccessToken
        {
            get { return isShowAccessToken; }
            set { SetProperty(ref isShowAccessToken, value); }
        }

        public string CredentialsProviderBing
        {
            get { return credentialsProviderBing; }
            set { SetProperty(ref credentialsProviderBing, value); }
        }

        public bool IsShowCredentialsProviderBing
        {
            get { return isShowCredentialsProviderBing; }
            set { SetProperty(ref isShowCredentialsProviderBing, value); }
        }

        public ICommand CheckAccessTokenCommand { get; }

        public ICommand CredentialsProviderBingChangedCommand { get; }

        public ICommand CancelButtonCommand { get; }

        public ICommand OkButtonCommand { get; }

        public SettingsViewModel(ISettingService settingService, IRegionManager regionManager,
                                                                 IWebApiClient webApiClient,
                                                                 IEventAggregator eventAggregator,
                                                                 IModuleManager moduleManager,
                                                                 IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.settingService = settingService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;
            this.interactionRequestService = interactionRequestService;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            CredentialsProviderBingChangedCommand = new DelegateCommand<string>(CredentialsProviderBingChanged);
            CancelButtonCommand = new DelegateCommand(Exit);
            OkButtonCommand = new DelegateCommand(Ok);

            isShowAccessToken = true;
            IsShowCredentialsProviderBing = true;
        }

        private void CredentialsProviderBingChanged(string credentialsProvider)
        {
            IsShowCredentialsProviderBing = IsShowCredentialsProviderBing || String.IsNullOrEmpty(credentialsProvider);
        }

        protected override void Activate()
        {
            base.Activate();
            FillAccessToken();
            FillCredentialsProviderBing();
        }

        protected override void DeActivate()
        {
            base.DeActivate();

            isShowAccessToken = true;
            AccessToken = String.Empty;

            IsShowCredentialsProviderBing = true;
            CredentialsProviderBing = String.Empty;
        }

        private void FillAccessToken()
        {
            string accessToken = settingService.GetAccessToken();
            if (String.IsNullOrEmpty(accessToken))
                return;

            IsShowAccessToken = false;
            AccessToken = accessToken;
        }

        private void FillCredentialsProviderBing()
        {
            string credentialsProvider = settingService.GetCredentialsProviderBing();
            if (String.IsNullOrEmpty(credentialsProvider))
                return;

            IsShowCredentialsProviderBing = false;
            CredentialsProviderBing = credentialsProvider;
        }

        private void CheckAccessToken(string token)
        {
            LoadingChangeActivity(true);
            CheckAccessTokenAsync(token);
        }

        private async void CheckAccessTokenAsync(string token)
        {
            try
            {
                IsValidAccessToken = await Task.Run(() => webApiClient.LogOn(token));
                webApiClient.LogOff();
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                LoadingChangeActivity(false);
            }
        }

        private void Ok()
        {
            if (IsValidAccessToken.HasValue && IsValidAccessToken.Value || String.IsNullOrEmpty(AccessToken))
                settingService.SetAccessToken(AccessToken);

            settingService.SetCredentialsProviderBing(CredentialsProviderBing);

            Exit();
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}