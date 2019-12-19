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

        private void AddDomainMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FrameModel.Domains.Add(new Domain("новый домен"));
        }

        private void RemoveDomainMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void AddDomainValueMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedDomain.Values.Add("новое значение");
        }

        private void RemoveDomainValueMenuItem_Click(object sender, RoutedEventArgs e)
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
    }
}
