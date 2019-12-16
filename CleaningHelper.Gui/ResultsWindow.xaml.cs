using CleaningHelper.ViewModel;
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

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Логика взаимодействия для ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsViewModel ViewModel { get; set; }

        public ResultsWindow(SemanticNetwork semanticNetwork = null, Concept result = null, List<List<Concept>> inferringPath = null)
        {
            InitializeComponent();

            ViewModel = new ResultsViewModel(semanticNetwork, result, inferringPath);
            DataContext = ViewModel;
        }

        private void ExpandAll(bool isExpanded)
        {
            var firstItem = ResultsTreeView.Items.Cast<TreeViewItem>().FirstOrDefault();
            if (firstItem == null) return;

            var stack = new Stack<TreeViewItem>(firstItem.Items.Cast<TreeViewItem>());
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                foreach (var child in item.Items)
                {
                    var childContainer = child as TreeViewItem ?? item.ItemContainerGenerator.ContainerFromItem(child) as TreeViewItem;

                    stack.Push(childContainer);
                }

                item.Focus();
                item.IsSelected = true;
                item.IsExpanded = isExpanded;
            }
        }

        private void ExpandAllButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandAll(true);
        }

        private void CollapseAllButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandAll(false);
        }
    }
}
