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
using GraphSharp.Controls;
using QuickGraph;

namespace CleaningHelper.Gui
{
    /// <summary>
    /// Логика взаимодействия для EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public EditorWindow()
        {
            InitializeComponent();

            var graph = new BidirectionalGraph<object, IEdge<object>>();

            var vertexStrings = new[] {"Супер", "Визуализация", "Для", "Светланы Игоревны"};

            foreach (var vertexString in vertexStrings)
            {
                graph.AddVertex(vertexString);
            }

            graph.AddEdge(new Edge<object>(vertexStrings[0], vertexStrings[1]));
            graph.AddEdge(new Edge<object>(vertexStrings[1], vertexStrings[2]));
            graph.AddEdge(new Edge<object>(vertexStrings[2], vertexStrings[3]));

            GraphLayout.Graph = graph;
            GraphLayout.LayoutMode = LayoutMode.Automatic;
        }

        private void GraphLayout_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is VertexControl vertexControl)
                MessageBox.Show(vertexControl.Vertex.ToString());
        }
    }
}
