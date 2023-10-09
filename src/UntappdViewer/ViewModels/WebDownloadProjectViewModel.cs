﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Interfaces;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using Checkin = UntappdViewer.Models.Checkin;
using Untappd = UntappdViewer.Views.Untappd;

namespace UntappdViewer.ViewModels
{
    public class WebDownloadProjectViewModel : LoadingBaseModel
    {
        private bool? accessToken;

        private string offsetUpdateBeer;

        private bool isEnabledFillServingTypeButton;

        private bool isEnabledCollaborationButton;

        private List<Checkin> checkins;

        private IUntappdService untappdService;

        private IWebApiClient webApiClient;

        private IModuleManager moduleManager;

        private ISettingService settingService;

        private IInteractionRequestService interactionRequestService;

        private ICancellationToken<Checkin> webClientCancellation;

        public ICommand CheckAccessTokenCommand { get; }

        public ICommand FulllDownloadButtonCommand { get; }

        public ICommand FirstDownloadButtonCommand { get; }

        public ICommand ToEndDownloadButtonCommand { get; }
        
        public ICommand BeerUpdateButtonCommand { get; }

        public ICommand FillServingTypeButtonCommand { get; }

        public ICommand FillCollaborationButtonCommand { get; }

        public ICommand OkButtonCommand { get; }

        public bool? AccessToken
        {
            get
            {
                return accessToken;
            }
            set
            {
                SetProperty(ref accessToken, value);
            }
        }

        public string OffsetUpdateBeer
        {
            get
            {
                return offsetUpdateBeer;
            }
            set
            {
                SetProperty(ref offsetUpdateBeer, value);
            }
        }

        public bool IsEnabledFillServingTypeButton
        {
            get { return isEnabledFillServingTypeButton; }
            set
            {
                SetProperty(ref isEnabledFillServingTypeButton, value);
            }
        }

        public bool IsEnabledCollaborationButton
        {
            get { return isEnabledCollaborationButton; }
            set
            {
                SetProperty(ref isEnabledCollaborationButton, value);
            }
        }

        public List<Checkin> Checkins
        {
            get
            {
                return checkins;
            }
            set
            {
                SetProperty(ref checkins, value);
                SetVisibilityFillServingTypeButton(value);
                SetVisibilityCollaborationButton();
            }
        }

        public WebDownloadProjectViewModel(IRegionManager regionManager, IUntappdService untappdService,
                                                                         IWebApiClient webApiClient,
                                                                         IModuleManager moduleManager,
                                                                         IEventAggregator eventAggregator,
                                                                         ISettingService settingService,
                                                                         IInteractionRequestService interactionRequestService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.untappdService = untappdService;
            this.webApiClient = webApiClient;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            this.interactionRequestService = interactionRequestService;

            CheckAccessTokenCommand = new DelegateCommand<string>(CheckAccessToken);
            FulllDownloadButtonCommand = new DelegateCommand(FulllDownloadCheckins);
            FirstDownloadButtonCommand = new DelegateCommand(FirstDownloadCheckins);
            ToEndDownloadButtonCommand = new DelegateCommand(ToEndDownloadCheckins);
            BeerUpdateButtonCommand = new DelegateCommand(UpdateBeers);

            FillServingTypeButtonCommand = new DelegateCommand(FillServingType);
            FillCollaborationButtonCommand = new DelegateCommand(FillCollaboration);

            webClientCancellation = webApiClient.GetCancellationToken<Checkin>();

            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            Checkins = new List<Checkin>(untappdService.GetCheckins());
            webApiClient.UploadedProgress += interactionRequestService.ShowMessageOnLoading;
            interactionRequestService.ClearMessageOnStatusBar();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Checkins.Clear();
            AccessToken = null;
            OffsetUpdateBeer = String.Empty;
            webApiClient.UploadedProgress -= interactionRequestService.ShowMessageOnStatusBar;
            interactionRequestService.ClearMessageOnStatusBar();
        }

        private void CheckAccessToken(string token)
        {
            LoadingChangeActivity(true);
            CheckAccessTokenAsync(token);
        }

        private async void CheckAccessTokenAsync(string token)
        {
            AccessToken = null;
            webApiClient.Initialize(token);
            try
            {
                AccessToken = await Task.Run(() => webApiClient.Check());
                if (AccessToken.HasValue && AccessToken.Value)
                {
                    long settingOffsetUpdateBeer = settingService.GetOffsetUpdateBeer();
                    if (settingOffsetUpdateBeer > 0)
                        OffsetUpdateBeer = settingOffsetUpdateBeer.ToString();

                    SetVisibilityCollaborationButton();
                }
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

        private void FulllDownloadCheckins()
        {
            if (untappdService.GetCheckins().Count  > 0 && !interactionRequestService.Ask(Properties.Resources.Warning, Properties.Resources.AskDeletedCheckins))
                return;

            Checkins.Clear();
            untappdService.GetCheckins().Clear();

            LoadingChangeActivity(true, true);
            FillCheckins(webApiClient.FillFullCheckins);
        }

        private void FirstDownloadCheckins()
        {
            LoadingChangeActivity(true, true);
            FillCheckins(webApiClient.FillFirstCheckins);
        }

        private void ToEndDownloadCheckins()
        {
            LoadingChangeActivity(true, true);
            FillCheckins(webApiClient.FillToEndCheckins);
        }

        private void UpdateBeers()
        {
            List<Beer> beers = untappdService.GetBeers();
            if (!beers.Any())
                return;

            LoadingChangeActivity(true, true);
            UpdateBeers(beers);
        }

        private void FillServingType()
        {
            List<Checkin> checkins = untappdService.GetCheckins().Where(item => String.IsNullOrEmpty(item.ServingType)).ToList();
            if (!checkins.Any())
                return;

            LoadingChangeActivity(true, true);
            FillServingType(checkins);
        }

        private void FillCollaboration()
        {
            List<Beer> beers = untappdService.GetBeers().Where(item => item.Collaboration.State == CollaborationState.Undefined).ToList();
            if (!beers.Any())
                return;

            LoadingChangeActivity(true, true);
            FillCollaboration(beers);
        }

        private async void FillCheckins(Action<CheckinsContainer, ICancellationToken<Checkin>> fillCheckinsDelegate)
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => fillCheckinsDelegate(untappdService.Untappd.CheckinsContainer, webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private async void UpdateBeers(List<Beer> beers)
        {
            BeforeRunWebClient();
            long offset = GetOffsetUpdateBeer();
            try
            {
                await Task.Run(() => webApiClient.UpdateBeers(beers, null, ref offset, webClientCancellation));
                SetOffsetUpdateBeer(webClientCancellation.Cancel ? offset : 0);
            }
            catch (Exception ex)
            {
                SetOffsetUpdateBeer(offset);
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private async void FillServingType(List<Checkin> checkins)
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => webApiClient.FillServingType(checkins, DefaultValues.DefaultServingType, webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private async void FillCollaboration(List<Beer> beers)
        {
            BeforeRunWebClient();
            try
            {
                await Task.Run(() => webApiClient.FillCollaboration(beers, untappdService.GetFullBreweries(), webClientCancellation));
            }
            catch (Exception ex)
            {
                interactionRequestService.ShowError(Properties.Resources.Error, StringHelper.GetFullExceptionMessage(ex));
            }
            finally
            {
                AfterRunWebClient();
            }
        }

        private void BeforeRunWebClient()
        {
            eventAggregator.GetEvent<LoadingCancel>().Subscribe(LoadingCanceled);
            interactionRequestService.ShowMessageOnStatusBar(String.Empty);
        }

        private void AfterRunWebClient()
        {
            webClientCancellation.Cancel = false;
            eventAggregator.GetEvent<LoadingCancel>().Unsubscribe(LoadingCanceled);

            untappdService.SortDataDescCheckins();
            Checkins = new List<Checkin>(untappdService.GetCheckins());

            interactionRequestService.ShowMessageOnStatusBar(interactionRequestService.GetCurrentMessageOnLoading());
            LoadingChangeActivity(false);
        }

        private void LoadingCanceled()
        {
            webClientCancellation.Cancel = true;
        }

        private long GetOffsetUpdateBeer()
        {
            return Int64.TryParse(OffsetUpdateBeer, out var offset) ? offset : 0;
        }

        private void SetOffsetUpdateBeer(long offset)
        {
            settingService.SetOffsetUpdateBeer(offset);
            OffsetUpdateBeer = offset > 0 ? offset.ToString() : String.Empty;
        }

        private void SetVisibilityFillServingTypeButton(List<Checkin> checkins)
        {
            IsEnabledFillServingTypeButton = checkins != null && checkins.Any(item => String.IsNullOrEmpty(item.ServingType));
        }

        private void SetVisibilityCollaborationButton()
        {
            List<Beer> beers = untappdService.GetBeers();
            IsEnabledCollaborationButton = AccessToken.HasValue && AccessToken.Value && beers != null && beers.Any(item => item.Collaboration.State == CollaborationState.Undefined);
        }

        private void Exit()
        {
            if (AccessToken.HasValue && AccessToken.Value)
            {
                long offset = GetOffsetUpdateBeer();
                if (offset != settingService.GetOffsetUpdateBeer())
                    settingService.SetOffsetUpdateBeer(offset);
            }

            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}