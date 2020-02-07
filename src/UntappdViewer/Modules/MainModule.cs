﻿using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Views;

namespace UntappdViewer.Modules
{
    public class MainModule : IModule
    {
        private IRegionManager regionManager;

        public MainModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion(RegionNames.RootRegion, () => containerProvider.Resolve<Main>());
        }
    }
}