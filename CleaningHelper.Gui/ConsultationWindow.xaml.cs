using CleaningHelper.ViewModel;
using System.Windows;

namespace CleaningHelper.Gui
{
    public partial class ConsultationWindow : Window
    {
        internal MainViewModel ViewModel { get; set; }

        public ConsultationWindow()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}