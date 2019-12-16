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
        private bool _isExpanded = false;

        public ResultsViewModel ViewModel { get; set; }

        public ResultsWindow(SemanticNetwork semanticNetwork = null, Concept result = null, List<List<Concept>> inferringPath = null)
        {
            InitializeComponent();

            ViewModel = new ResultsViewModel(semanticNetwork, result, inferringPath);
            DataContext = ViewModel;
        }

        private void ExpandAll(TreeViewItem treeViewItem, bool isExpanded = true)
        {
            var stack = new Stack<TreeViewItem>(treeViewItem.Items.Cast<TreeViewItem>());
            while (stack.Count > 0)
            {
                TreeViewItem item = stack.Pop();

                foreach (var child in item.Items)
                {
                    var childContainer = child as TreeViewItem;
                    if (childContainer == null)
                    {
                        childContainer = item.ItemContainerGenerator.ContainerFromItem(child) as TreeViewItem;
                    }

                    stack.Push(childContainer);
                }

                item.IsExpanded = isExpanded;
                item.ExpandSubtree();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _isExpanded = !_isExpanded;
            var firstItem = ResultsTreeView.Items[0] as TreeViewItem;
            if (firstItem == null)
                return;
            ExpandAll(firstItem, _isExpanded);
        }
    }
}
