﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain;
using UntappdViewer.Domain.Models;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Utils;
using UntappdViewer.Views;
using Checkin = UntappdViewer.Models.Checkin;

namespace UntappdViewer.ViewModels
{
    public class StatisticsProjectViewModel : RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IUntappdService untappdService;

        private IEnumerable chekinRatingScore;

        private IEnumerable beerRatingScore;

        private IEnumerable dateChekins;

        private IEnumerable beerTypeCount;

        private IEnumerable beerTypeRating;

        private IEnumerable beerCountryCount;

        private IEnumerable beerCountryRating;

        private int totalCheckinCount;

        private int uniqueCheckinCount;

        private int breweryCount;

        private int countryCount;

        private int maxYAxisRatingScore;

        private double minWidthChartDateChekins;

        private int maxXAxisBeerTypeCount;

        private double maxXAxisBeerTypeRating;

        private int heightChartBeerType;

        private int maxXAxisBeerCountryCount;

        private double maxXAxisBeerCountryRating;

        private int heightChartBeerCountry;

        private double averageChekinRating;

        private double averageBeerRating;

        private int totalDays;

        private double averageChekinsQuantity;

        private double averageUniqueChekinsQuantity;


        public ICommand OkButtonCommand { get; }

        public IEnumerable ChekinRatingScore
        {
            get { return chekinRatingScore; }
            set
            {
                SetProperty(ref chekinRatingScore, value);
            }
        }

        public IEnumerable BeerRatingScore
        {
            get { return beerRatingScore; }
            set
            {
                SetProperty(ref beerRatingScore, value);
            }
        }

        public IEnumerable DateChekins
        {
            get { return dateChekins; }
            set
            {
                SetProperty(ref dateChekins, value);
            }
        }

        public IEnumerable BeerTypeCount
        {
            get { return beerTypeCount; }
            set
            {
                SetProperty(ref beerTypeCount, value);
            }
        }

        public IEnumerable BeerTypeRating
        {
            get { return beerTypeRating; }
            set
            {
                SetProperty(ref beerTypeRating, value);
            }
        }

        public IEnumerable BeerCountryCount
        {
            get { return beerCountryCount; }
            set
            {
                SetProperty(ref beerCountryCount, value);
            }
        }

        public IEnumerable BeerCountryRating
        {
            get { return beerCountryRating; }
            set
            {
                SetProperty(ref beerCountryRating, value);
            }
        }
        public int TotalCheckinCount
        {
            get { return totalCheckinCount; }
            set
            {
                SetProperty(ref totalCheckinCount, value);
            }
        }

        public int UniqueCheckinCount
        {
            get { return uniqueCheckinCount; }
            set
            {
                SetProperty(ref uniqueCheckinCount, value);
            }
        }

        public int BreweryCount
        {
            get { return breweryCount; }
            set
            {
                SetProperty(ref breweryCount, value);
            }
        }

        public int CountryCount
        {
            get { return countryCount; }
            set
            {
                SetProperty(ref countryCount, value);
            }
        }

        public int MaxYAxisRatingScore
        {
            get { return maxYAxisRatingScore; }
            set
            {
                SetProperty(ref maxYAxisRatingScore, value);
            }
        }

        public double MinWidthChartDateChekins
        {
            get { return minWidthChartDateChekins; }
            set
            {
                SetProperty(ref minWidthChartDateChekins, value);
            }
        }

        public int MaxXAxisBeerTypeCount
        {
            get { return maxXAxisBeerTypeCount; }
            set
            {
                SetProperty(ref maxXAxisBeerTypeCount, value);
            }
        }

        public double MaxXAxisBeerTypeRating
        {
            get { return maxXAxisBeerTypeRating; }
            set
            {
                SetProperty(ref maxXAxisBeerTypeRating, value);
            }
        }

        public int HeightChartBeerType
        {
            get { return heightChartBeerType; }
            set
            {
                SetProperty(ref heightChartBeerType, value);
            }
        }

        public int MaxXAxisBeerCountryCount
        {
            get { return maxXAxisBeerCountryCount; }
            set
            {
                SetProperty(ref maxXAxisBeerCountryCount, value);
            }
        }
        public double MaxXAxisBeerCountryRating
        {
            get { return maxXAxisBeerCountryRating; }
            set
            {
                SetProperty(ref maxXAxisBeerCountryRating, value);
            }
        }

        public int HeightChartBeerCountry
        {
            get { return heightChartBeerCountry; }
            set
            {
                SetProperty(ref heightChartBeerCountry, value);
            }
        }

        public double AverageChekinRating
        {
            get { return averageChekinRating; }
            set
            {
                SetProperty(ref averageChekinRating, value);
            }
        }

        public double AverageBeerRating
        {
            get { return averageBeerRating; }
            set
            {
                SetProperty(ref averageBeerRating, value);
            }
        }

        public int TotalDays
        {
            get { return totalDays; }
            set
            {
                SetProperty(ref totalDays, value);
            }
        }

        public double AverageChekinsQuantity
        {
            get { return averageChekinsQuantity; }
            set
            {
                SetProperty(ref averageChekinsQuantity, value);
            }
        }

        public double AverageUniqueChekinsQuantity
        {
            get { return averageUniqueChekinsQuantity; }
            set
            {
                SetProperty(ref averageUniqueChekinsQuantity, value);
            }
        }

        public StatisticsProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager,
                                                                        IUntappdService untappdService) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.untappdService = untappdService;

            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            SetCountsPanel();
            SetRatingScore();
            SetDataCheckins();
            SetBeerType();
            SetBeerCountry();
        }

        protected override void DeActivate()
        {
            base.DeActivate();

            TotalCheckinCount =0;
            UniqueCheckinCount = 0;
            BreweryCount = 0;
            CountryCount = 0;

            MaxYAxisRatingScore = 0;
            ChekinRatingScore = null;
            AverageChekinRating = 0;
            BeerRatingScore = null;
            AverageBeerRating = 0;

            MinWidthChartDateChekins = 0;
            DateChekins = null;
            TotalDays = 0;
            AverageChekinsQuantity = 0;
            AverageUniqueChekinsQuantity = 0;

            HeightChartBeerType = 0;
            MaxXAxisBeerTypeCount = 0;
            BeerTypeCount = null;
            MaxXAxisBeerTypeRating = 0;
            BeerTypeRating = null;

            HeightChartBeerCountry = 0;
            MaxXAxisBeerCountryCount = 0;
            BeerCountryCount = null;
            MaxXAxisBeerCountryRating = 0;
            BeerCountryRating = null;
        }

        private void SetCountsPanel()
        {
            List<Checkin> checkins = untappdService.GetCheckins();
            TotalCheckinCount = checkins.Count;

            UniqueCheckinCount = untappdService.GetCheckins(true).Count;
            BreweryCount = untappdService.GetBrewerys().Count;
            CountryCount = StatisticsCalculation.GetCountrysCount(checkins);
        }

        private void SetRatingScore()
        {
            List<KeyValue<double, int>> chekinRatingScore = StatisticsCalculation.GetChekinRatingScore(untappdService.GetCheckins());
            List<KeyValue<double, int>> beerRatingScore = StatisticsCalculation.GetBeerRatingScore(untappdService.GetBeers());

            int chekinMaxCount = chekinRatingScore.Count >0 ? chekinRatingScore.Select(item => item.Value).Max() : 0;
            int beerMaxCount = beerRatingScore.Count > 0 ? beerRatingScore.Select(item => item.Value).Max() : 0;
            MaxYAxisRatingScore = Math.Max(chekinMaxCount, beerMaxCount) + 100;

            ChekinRatingScore = chekinRatingScore;
            AverageChekinRating = Math.Round(MathHelper.GetAverageValue(ConverterHelper.KeyValuesToDictionary(chekinRatingScore)), 2);

            BeerRatingScore = beerRatingScore;
            AverageBeerRating = Math.Round(MathHelper.GetAverageValue(ConverterHelper.KeyValuesToDictionary(beerRatingScore)), 2);
        }

        private void SetDataCheckins()
        {
            List<Checkin> checkins = untappdService.GetCheckins();
            List<KeyValue<string, int>> dateChekins = StatisticsCalculation.GetDateChekins(checkins);
            MinWidthChartDateChekins = MathHelper.GetCeilingByStep(dateChekins.Count * 15, 100);
            DateChekins = dateChekins;

            TotalDays = StatisticsCalculation.GetTotalDays(checkins);
            AverageChekinsQuantity = Math.Round(StatisticsCalculation.GetAverageQuantity(checkins), 2);
            AverageUniqueChekinsQuantity = Math.Round(StatisticsCalculation.GetAverageQuantity(untappdService.GetCheckins(true)), 2);
        }

        private void SetBeerType()
        {
            List<Checkin> checkins = untappdService.GetCheckins();
            List<KeyValue<string, List<long>>> beerTypeCheckinIds = StatisticsCalculation.GetBeerTypeCheckinIdGroupByCount(checkins);

            HeightChartBeerType = MathHelper.GetCeilingByStep(beerTypeCheckinIds.Count * DefautlValues.HeightBarSeriesChartYDelta, 100);

            List <KeyValue<string, int>> beerTypeCount = StatisticsCalculation.GetListCount(beerTypeCheckinIds);
            MaxXAxisBeerTypeCount = beerTypeCount.Count > 0 ? beerTypeCount.Max(item => item.Value) + 20 : 0;
            BeerTypeCount = beerTypeCount;

            List<KeyValue<string, double>> beerTypeRating = StatisticsCalculation.GetCheckinRatingByIds(checkins, beerTypeCheckinIds);
            MaxXAxisBeerTypeRating = beerTypeRating.Count > 0 ? beerTypeRating.Max(item => item.Value) + 0.1 : 0;
            BeerTypeRating = beerTypeRating;
        }

        private void SetBeerCountry()
        {
            List<Checkin> checkins = untappdService.GetCheckins();
            List<KeyValue<string, List<long>>> beerCountryCheckinIds = StatisticsCalculation.GetCountryCheckinIds(checkins);

            HeightChartBeerCountry = MathHelper.GetCeilingByStep(beerCountryCheckinIds.Count * DefautlValues.HeightBarSeriesChartYDelta, 100);

            List<KeyValue<string, int>> beerCountryCount = StatisticsCalculation.GetListCount(beerCountryCheckinIds);
            MaxXAxisBeerCountryCount = beerCountryCount.Count > 0 ? beerCountryCount.Max(item => item.Value) + 20 : 0;
            BeerCountryCount = beerCountryCount;

            List<KeyValue<string, double>> beerCountryRating = StatisticsCalculation.GetCheckinRatingByIds(checkins, beerCountryCheckinIds);
            MaxXAxisBeerCountryRating = beerCountryRating.Count > 0 ? beerCountryRating.Max(item => item.Value) + 0.1 : 0;
            BeerCountryRating = beerCountryRating;
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}