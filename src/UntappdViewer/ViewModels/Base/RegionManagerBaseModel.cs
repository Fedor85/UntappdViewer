﻿using System;
using System.Linq;
using Prism.Regions;

namespace UntappdViewer.ViewModels
{
    public abstract class RegionManagerBaseModel : ActiveAwareBaseModel
    {
        protected IRegionManager regionManager;

        protected RegionManagerBaseModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        protected void ActivateView(string regionName, Type viewType)
        {
            IRegion region = regionManager.Regions[regionName];
            object view = region.Views.First(i => i.GetType().Equals(viewType));
            region.Activate(view);
        }

        protected void DeActivateAllViews(string regionName)
        {
            IRegion region = regionManager.Regions[regionName];
            foreach (object activeView in region.ActiveViews)
                region.Deactivate(activeView);
        }
    }
}