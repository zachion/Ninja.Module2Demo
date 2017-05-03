using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;


namespace NinjaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class MainWindow : Window
    {
        private readonly ConnectedRespository _repo = new ConnectedRespository();
        private Ninja _currentNinja;
        private bool _isLoading;
        private bool _isNinjaListChanging;
        private ObjectDataProvider _ninjaViewSource;
        private ObservableCollection<NinjaEquipment> _objervableEquipment = new ObservableCollection<NinjaEquipment>();

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style) FindResource(typeof(Window));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoading = false;
            NinjaListbox.ItemsSource = _repo.NinjasInMemory();
            SortNinjaList();
            clanComboBox.ItemsSource = _repo.GetClanList();
            _ninjaViewSource = ((ObjectDataProvider)(FindResource("ninjaViewSource")));
            NinjaListbox.SelectedIndex = 0;
            _isLoading = false;
        }
        private void SortNinjaList()
        {
            var dataView = CollectionViewSource.GetDefaultView(NinjaListbox.ItemsSource);
            dataView.SortDescriptions.Clear();
            var sd = new SortDescription("Name", ListSortDirection.Ascending);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
            NinjaListbox.SelectedItem = _currentNinja;
        }

        private void ninjaListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool continueProcess;
            if (_isLoading)
            {
                continueProcess = true;
            }
            else
            {
                continueProcess = ShouldRefresh;
            }
            if (!continueProcess) return;
            _currentNinja = _repo.GetNinjaWithEquipment((int) NinjaListbox.SelectedValue);
            RefreshNinja();
            _isNinjaListChanging = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _repo.Save();
            SortNinjaList();
        }

        private bool ShouldRefresh
        {
            get
            {
                var continueProcess = true;
                if (_currentNinja != null)
                {
                    if (_currentNinja.IsDirty)
                    {
                        switch (MessageBox.Show("Save current ninja?", "Ninja Entry", MessageBoxButton.YesNoCancel))
                        {
                                case MessageBoxResult.Cancel:
                                    continueProcess = false;
                                    break;
                                case MessageBoxResult.Yes:
                                    _repo.Save();
                                    break;
                                case MessageBoxResult.No:
                                    break;

                        }
                    }
                }
                return continueProcess;
            }
        }
        private void RefreshNinja()
        {
            _isNinjaListChanging = true;
            _ninjaViewSource.ObjectInstance = _currentNinja;
            _objervableEquipment = new ObservableCollection<NinjaEquipment>(_currentNinja.EquipmentOwned);
            equipmentOwnedDataGrid.ItemsSource = _objervableEquipment;
            _objervableEquipment.CollectionChanged += EquipmentCollectionChanged;

            clanComboBox.SelectedValue = _currentNinja.ClanId;
            _currentNinja.IsDirty = false;
            _isNinjaListChanging = false;

        }
        private void EquipmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isLoading || _isNinjaListChanging)
            {
                return;
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                _repo.DeleteQuipment(e.OldItems);
                SetNinjaDirty();
            }
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _currentNinja.EquipmentOwned.AddRange(e.NewItems.Cast<NinjaEquipment>());
                SetNinjaDirty();
            }
        }

        private void btnNewNinja_Click(object sender, RoutedEventArgs e)
        {
            if (ShouldRefresh)
            {
                _currentNinja = _repo.NewNinja();
                RefreshNinja();
            }

        }

        private void clanComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoading && !_isNinjaListChanging)
            {
                _currentNinja.ClanId = (int) clanComboBox.SelectedValue;

            }
            SetNinjaDirty();
        }

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetNinjaDirty();
        }

        private void DateOfBirthDatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetNinjaDirty();
        }

        private void ServedInOniwabanCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SetNinjaDirty();
        }

        private void SetNinjaDirty()
        {
            if (!_isLoading && !_isNinjaListChanging)
            {
                _currentNinja.IsDirty = true;
            }
        }

        
    }
    
}
