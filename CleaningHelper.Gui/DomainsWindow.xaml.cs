using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using CleaningHelper.Model;
using CleaningHelper.Model.Exceptions;
using CleaningHelper.ViewModel;

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Логика взаимодействия для DomainsWindow.xaml
    /// </summary>
    public partial class DomainsWindow : Window
    {
        public DomainsViewModel ViewModel { get; set; }

        public DomainsWindow(FrameModel frameModel)
        {
            InitializeComponent();

            ViewModel = new DomainsViewModel(frameModel);
            DataContext = ViewModel;
        }

        private void RemoveValueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var usedList = ViewModel.FrameModel.CheckDomainValueIntegrity(ViewModel.SelectedValue);

            if (usedList.Count > 0)
            {
                var message =
                    $"Значение домена используется:" +
                    $"{Environment.NewLine}" +
                    $"{string.Join(Environment.NewLine, usedList.Select(t => $"{t.Item1}.{t.Item2}"))}" +
                    $"Значения этих слотов будут заменены на пустые";

                var messageBoxResult = MessageBox.Show(message, "Предупреждение", MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.No)
                    return;
            }

            ViewModel.SelectedDomain.Values.Remove(ViewModel.SelectedValue);
            ViewModel.FrameModel.RestoreDomainValueIntegrity();
        }

        private void AddValueButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (ViewModel.SelectedValue == null)
            {
                ViewModel.SelectedDomain.Values.Add("Новое значение");
            }
            else
            {
                var index = ViewModel.SelectedDomain.Values.IndexOf(ViewModel.SelectedValue);
                ViewModel.SelectedDomain.Values.Insert(index, new DomainValue("Новое значение"));
            }
        }

        private void AddDomainButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedDomain == null)
            {
                ViewModel.FrameModel.Domains.Add(new Domain("Новый домен"));
            }
            else
            {
                var index = ViewModel.FrameModel.Domains.IndexOf(ViewModel.SelectedDomain);
                ViewModel.FrameModel.Domains.Insert(index, new Domain("Новый домен"));
            }
        }

        private void RemoveDomainButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.FrameModel.Domains.Remove(ViewModel.SelectedDomain);
            }
            catch (DomainIsInUseException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
