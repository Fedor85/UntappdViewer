﻿using System;
using System.Collections.Generic;
using System.IO;
using UntappdViewer.Infrastructure;
using UntappdViewer.Models;
using UntappdViewer.Models.Different;
using UntappdViewer.UI.Controls.ViewModel;
using UntappdViewer.Utils;

namespace UntappdViewer.Helpers
{
    public static class ConverterHelper
    {
        public static string GetServingTypeImagePath(string servingTypeName)
        {
            if (servingTypeName == null)
                return DefaultValues.EmptyImage;

            switch (servingTypeName.ToLower())
            {
                case "draft":
                    return @"..\Resources\ServingType\draft@3x.png";
                case "bottle":
                    return @"..\Resources\ServingType\bottle@3x.png";
                case "can":
                    return @"..\Resources\ServingType\can@3x.png";
                case "taster":
                    return @"..\Resources\ServingType\taster@3x.png";
                case "cask":
                    return @"..\Resources\ServingType\cask@3x.png";
                case "crowler":
                    return @"..\Resources\ServingType\crowler.png";
                case "growler":
                    return @"..\Resources\ServingType\growler.png";
                default:
                    return DefaultValues.EmptyImage;
            }
        }

        #region GalleryProject

        public static RatingViewModel GetCheckinViewModel(Checkin checkin, string photoPath)
        {
            RatingViewModel ratingViewModel = new RatingViewModel();
            ratingViewModel.Caption = checkin.Beer.Name;
            ratingViewModel.ImagePath = File.Exists(photoPath) ? photoPath : DefaultValues.NoImageIconResources;
            ratingViewModel.RatingScore = checkin.RatingScore ?? 0;
            return ratingViewModel;
        }

        public static RatingViewModel GetBeerViewModel(Beer beer, string labelPath)
        {
            RatingViewModel ratingViewModel = new RatingViewModel();
            ratingViewModel.Caption = beer.Name;
            if (File.Exists(labelPath))
                ratingViewModel.ImagePath = labelPath;

            ratingViewModel.RatingScore = beer.GlobalRatingScore;
            return ratingViewModel;
        }

        public static ImageViewModel GetBreweryViewModel(Brewery brewery, string labelPath)
        {
            ImageViewModel imageViewModel = new ImageViewModel();
            imageViewModel.Caption = brewery.Name;
            if (File.Exists(labelPath))
                imageViewModel.ImagePath = labelPath;

            if (brewery.Venue != null && !String.IsNullOrEmpty(brewery.Venue.Country))
                imageViewModel.Description = brewery.Venue.Country;

            return imageViewModel;
        }

        public static ImageViewModel GetBadgeViewModel(Badge badge, string imagePath)
        {
            ImageViewModel imageViewModel = new ImageViewModel();
            imageViewModel.Caption = badge.Name;
            if (File.Exists(imagePath))
                imageViewModel.ImagePath = imagePath;

            if (!String.IsNullOrEmpty(badge.Description))
                imageViewModel.Description = StringHelper.GetSplitByLength(badge.Description, 40);

            return imageViewModel;
        }

        public static Dictionary<T1, T3> KeyValueToDirectory<T1, T2, T3>(List<KeyValue<T1, T2>> values)
        {
            Dictionary<T1, T3> dictionary = new Dictionary<T1, T3>();
            foreach (KeyValue<T1, T2> value in values)
            {
                if (!dictionary.ContainsKey(value.Key))
                    dictionary.Add(value.Key, ParserAndConvertHelper.GetConvertValue<T3>(value.Value));
            }
            return dictionary;
        }

        #endregion

        public static Dictionary<string, T> ConvertNameToCode<T>(Dictionary<string, T> countries)
        {
            Dictionary<string, T> countryCodes = new Dictionary<string, T>();
            foreach (KeyValuePair<string, T> country in countries)
            {
                string code = CountryNameHelper.GetCountryCode(country.Key);
                countryCodes.Add(String.IsNullOrEmpty(code) ? country.Key : code, country.Value);
            }
            return countryCodes;
        }
    }
}