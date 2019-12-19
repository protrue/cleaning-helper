using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using CleaningHelper.ViewModel;
using System.Windows;
using CleaningHelper.Model;

namespace CleaningHelper.Gui
{
    public partial class ConsultationWindow : Window
    {
        public ConsultationViewModel ViewModel { get; set; }
        
        public ConsultationWindow(FrameModel frameModel)
        {
            InitializeComponent();

            ViewModel = new ConsultationViewModel(frameModel);
            DataContext = ViewModel;
        }
    }
}