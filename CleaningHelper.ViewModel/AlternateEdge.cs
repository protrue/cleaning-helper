using System;
using System.Windows.Media;
using GraphSharp;

namespace CleaningHelper.ViewModel
{
    public class AlternateEdge : TypedEdge<Object>
    {
        public String Id { get; set; }

        public Color EdgeColor { get; set; }

        public AlternateEdge(Object source, Object target) : base(source, target, EdgeTypes.General) { }
    }
}
