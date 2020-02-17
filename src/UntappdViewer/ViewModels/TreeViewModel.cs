﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : RegionManagerBaseModel
    {
        private UntappdService untappdService;

        private IModuleManager moduleManager;

        private ISettingService settingService;

        private List<TreeViewItem> treeItems;

        private TreeViewItem selectedTreeItem;

        private string treeViewCaption;

        private bool isCheckedUniqueCheckBox;

        public ICommand UniqueCheckedCommand { get; }
   
        public string TreeViewCaption
        {
            get { return treeViewCaption; }
            set
            {
                treeViewCaption = value;
                OnPropertyChanged();
            }
        }

        public bool IsCheckedUniqueCheckBox
        {
            get { return isCheckedUniqueCheckBox; }
            set
            {
                isCheckedUniqueCheckBox = value;
                OnPropertyChanged();
            }
        }

        public List<TreeViewItem> TreeItems
        {
            get { return treeItems; }
            set
            {
                treeItems = value;
                OnPropertyChanged();
            }
        }

        public TreeViewItem SelectedTreeItem
        {
            get { return selectedTreeItem; }
            set
            {
                selectedTreeItem = value;
                OnPropertyChanged();
                UpdateContent();
            }
        }

        public TreeViewModel(UntappdService untappdService, IModuleManager moduleManager, IRegionManager regionManager, ISettingService settingService) : base(regionManager)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.settingService = settingService;
            UniqueCheckedCommand = new DelegateCommand<bool?>(UniqueChecked);
            TreeItems = new List<TreeViewItem>();
        }

        protected override void Activate()
        {
            base.Activate();
            IsCheckedUniqueCheckBox = settingService.GetIsCheckedUniqueCheckBox();
            untappdService.UpdateUntappdEvent += UpdateTree;
            UpdateTree();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            untappdService.UpdateUntappdEvent -= UpdateTree;
            settingService.SetIsCheckedUniqueCheckBox(IsCheckedUniqueCheckBox);
            DeActivateAllViews(RegionNames.ContentRegion);
            SaveSelectedTreeItem();
            TreeItems.Clear();
        }

        private void UpdateContent()
        {
            moduleManager.LoadModule(typeof(CheckinModule).Name);
            ActivateView(RegionNames.ContentRegion, typeof(Checkin));
        }

        private void UniqueChecked(bool? isChecked)
        {
            if (isChecked.HasValue)
            {
                SaveSelectedTreeItem();
                UpdateTree(isChecked.Value);
            }
        }

        private void UpdateTree()
        {
            UpdateTree(IsCheckedUniqueCheckBox);
        }

        private void UpdateTree(bool isUniqueCheckins)
        {
            TreeItems = untappdService.GeTreeViewItems(isUniqueCheckins);
            SetSelectedTreeItem();
            TreeViewCaption = $"{Properties.Resources.Checkins} ({TreeItems.Count}):";
        }

        private void SetSelectedTreeItem()
        {
            long selectedTreeItemId = settingService.GetSelectedTreeItemId();
            TreeViewItem selectedTreeItem = TreeItems.FirstOrDefault(item => item.Id.Equals(selectedTreeItemId));
            if (selectedTreeItem != null)
                SelectedTreeItem = selectedTreeItem;
        }

        private void SaveSelectedTreeItem()
        {
            if (SelectedTreeItem != null)
                settingService.SetSelectedTreeItemId(SelectedTreeItem.Id);
        }
    }
}