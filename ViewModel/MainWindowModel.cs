using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackageLoader.Core.Algorytm;
using PackageLoader.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PackageLoader.ViewModel
{
    public class MainWindowModel : ObservableObject
    {
        #region Private properties

        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private string _message;
        private string _errors;
        private Item _item;
        #endregion

        #region Public properties

        public ObservableCollection<Item> Items
        {
            get => _items;
            set { SetProperty(ref _items, value); OnPropertyChanged(nameof(Items)); }
        }
        public Item Item
        {
            get => _item;
            set { SetProperty(ref _item, value); OnPropertyChanged(nameof(Item)); }
        }

        public string Message
        {
            get => _message;
            set { SetProperty(ref _message, value); OnPropertyChanged(nameof(Message)); }
        }

        public string Errors
        {
            get => _errors;
            set { SetProperty(ref _errors, value); OnPropertyChanged(nameof(Errors)); }
        }

        public double PackageWidth { get; set; } = 20;
        public double PackageHeight { get; set; } = 20;
        public double PackageDepth { get; set; } = 20;
        public double PackageWeight { get; set; } = 100;

        #endregion

        #region Commands

        public ICommand RunCommand { get; private set; }

        public ICommand AddItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }

        public ICommand ClearErrorCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
            DeleteItemCommand = new RelayCommand(() => DeleteItem(Item));
            ClearErrorCommand = new RelayCommand(() => Errors = null);
            RunCommand = new RelayCommand<EAlgorytmType>(Run);
        }

        #endregion

        #region Methods

        private void Run(EAlgorytmType type)
        {
            try
            {

                if (Items.Count <= 0)
                    return;
                Message = null;

                List<Package> packages = null;
               
                switch (type)
                {
                    case EAlgorytmType.Knapsack:
                        AddMessage($"Pakuje przy użyciu algorytpu plecakowego");
                        var knapsack = new KnapsackSolver();
                        AddMessage($"...");
                        packages = knapsack.PackItems(Items.ToList(), PackageWidth, PackageHeight, PackageDepth, PackageWeight);
                        break;
                    case EAlgorytmType.BFD:
                        AddMessage($"Pakuje przy użyciu algorytpu BFD");
                        var BFD = new BestFitDecreasingg();
                        AddMessage($"...");
                        packages = BFD.PackItems(Items.ToList(), PackageWidth, PackageHeight, PackageDepth, PackageWeight);
                        break;
                }
                AddMessage($"");
                if (packages != null)
                {
                    int packageNumber = 1;
                    foreach (var package in packages)
                    {
                        AddMessage($"Package {packageNumber++}:");
                        foreach (var item in package.GetItems())
                        {
                            AddMessage($"- {item.Name}: {item.Width}x{item.Height}x{item.Depth}, {item.Weight}kg");
                        }
                        AddMessage(package.ToString());
                        AddMessage("");
                    }
                }
            }
            catch (Exception ex)
            {
                CatchExeption(ex);
            }
        }

        private void AddMessage(string message)
        {
            if(Message == null)
                Message = $"{message}";
            else
                Message += $"\n{message}";
        }

        private void CatchExeption(Exception ex)
        {
            Errors = ex.Message;
        }

        private void AddItem()
        {
            try
            {
                string name = $"Item[{Items.Count}]";
                double width = 0, height = 0, depth = 0, weight = 0;
                width = Random(1, 5);
                depth = Random(1, 5);
                height = Random(1, 5);
                weight = Random(1, 20);
                Items.Add(new Item(name, width, height, depth, weight));

            }catch (Exception ex) 
            {
                CatchExeption(ex);
            }
        }

        private void DeleteItem(Item item)
        {
            try
            {
                if (item == null) return;
                var index = Items.IndexOf(item);
                Items.Remove(item);
                if(index > 0)
                    Item = Items[index -1];
                else if(Items.Count > 0)
                    Item = Items[index];


            }
            catch (Exception ex)
            {
                CatchExeption(ex);
            }
        }

        private int Random(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        #endregion
    }
}