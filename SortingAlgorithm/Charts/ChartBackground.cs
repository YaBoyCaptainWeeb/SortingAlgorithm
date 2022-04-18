using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SortingAlgorithm.Charts
{
    internal class ChartBackground : Grid
    {
        private SolidColorBrush bg = new(Color.FromArgb(255, 180, 200, 180));
        private Grid grid = new();
        private Grid paddinggrid = new();

        public UIElementCollection ChartItems
        {
            get => paddinggrid.Children;
        }
        public ChartBackground()
        {
            Background = new SolidColorBrush(Color.FromArgb(150, 190, 220, 190));
            Children.Add(paddinggrid);
            paddinggrid.Margin = new(0);
            //DrawLine();
        }
        public void DrawLine()
        {
            for (int i = 0; i <= 6; i++)
            {
                Line line = new();
                line.X1 = 10;
                line.Y1 = -i * 50 + paddinggrid.Margin.Bottom;
                line.X2 = 450;
                line.Y2 = -i * 50 + paddinggrid.Margin.Bottom;
                line.Stroke = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
                line.StrokeDashArray = new DoubleCollection() { 5 };
                line.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                line.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Children.Add(line);
            }
        }
    }
}
