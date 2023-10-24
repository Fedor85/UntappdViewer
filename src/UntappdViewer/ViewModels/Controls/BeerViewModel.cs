﻿using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace UntappdViewer.ViewModels.Controls
{
    public class BeerViewModel
    {
        public string Url { get; set; }

        public bool VisibilityUrl
        {
            get { return !String.IsNullOrEmpty(Url); }
        }

        public string Name { get; set; }

        public BitmapSource Label { get; set; }

        public bool VisibilityLabel
        {
            get { return Label != null; }
        }

        public string Type { get; set; }

        public string ABV { get; set; }

        public string IBU { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }

        public bool VisibilityDescription
        {
            get { return !String.IsNullOrEmpty(Description); }
        }

        public List<BreweryViewModel> BreweryViewModels { get; }

        public BeerViewModel()
        {
            BreweryViewModels = new List<BreweryViewModel>();
        }
    }
}