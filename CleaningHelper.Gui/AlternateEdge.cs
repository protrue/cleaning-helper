using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GraphSharp;

namespace CleaningHelper.Gui
{
    public class AlternateEdge : TypedEdge<Object>
    {
        public String Id { get; set; }

        public Color EdgeColor { get; set; }

        public AlternateEdge(Object source, Object target) : base(source, target, EdgeTypes.General) { }
    }
}
