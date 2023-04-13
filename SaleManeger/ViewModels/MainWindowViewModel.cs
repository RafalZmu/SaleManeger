﻿using ReactiveUI;
using SaleManeger.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace SaleManeger.ViewModels
{
    public class MainWindowViewModel : ReactiveObject 
    {
        private string _saleName;
        private DataBase _dataBase { get; set; }
        private ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }


        public MainWindowViewModel()
        {
            _dataBase = new DataBase();
            OpenProjectSelection();
            Batteries.Init();
        }

        public void OpenEditProductsView()
        {
            var productView = new ProductEditionViewModel(_dataBase);
            Content = productView;
            productView.SaveToDataBaseCommand.Subscribe(model =>
            {
                OpenProjectSelection();
            });
        }

        public void OpenProjectSelection()
        {
            var projectViewModel = new ProjectSelectionViewModel(_dataBase);
            Content = projectViewModel;

            projectViewModel.CreateNewSaleCommand.Subscribe(model =>
            {
                OpenClientSelection(model);
            });
            projectViewModel.OpenSaleCommand.Subscribe(model =>
            {
                OpenClientSelection(model);
            });
        }
        public void OpenClientSelection(string saleName)
        {
            _saleName = saleName;
            var clientSelectionViewModel = new ClientSelectionViewModel(saleName, _dataBase);
            Content = clientSelectionViewModel;
            clientSelectionViewModel.OpenClientEditionCommand.Subscribe(model =>
            {
                OpenClientEdition(model);
            });
            clientSelectionViewModel.OpenProjectSelectionCommand.Subscribe(model =>
            {
                OpenProjectSelection();
            });
        }

        public void OpenClientEdition(Client client)
        {
            var clientEditionViewModel = new ClientEditionViewModel(_dataBase, client, _saleName);
            Content = clientEditionViewModel;

            clientEditionViewModel.OpenClientSelectionCommand.Subscribe(OpenClientSelection);

        }
    }
}