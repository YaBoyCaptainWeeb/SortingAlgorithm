using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace SortingAlgorithm.Charts
{
    internal abstract class Chart
    {
        private readonly double factor = 0.666666666666667;
        protected readonly double PaddingChart = 10;
        protected double WidthChart;
        protected double HeightChart;
        public readonly Canvas ChartBackground = new();
    public Chart()
        {
            ChartBackground.Margin = new Thickness(0);
            ChartBackground.SizeChanged += ChartBackground_SizeChanged;
        }
    private void ChartBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            WidthChart = e.NewSize.Width - (PaddingChart * 2);
            HeightChart = e.NewSize.Height * factor;
            ChartBackground.Background = DrawLines(e.NewSize.Width, WidthChart, PaddingChart);
        }
        public abstract void AddValue(int data);
        public abstract void Clear();
        private Brush DrawLines (double actualwidth, double widthchart, double padding)
        {
            double W = actualwidth;
            double w = widthchart;
            double offset = padding;
            double x = w / W;
            double delta = offset / W;
            int numLines = 10;
            DrawingBrush brush = new()
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(delta, 0, x / numLines, factor / numLines),
                Drawing = new GeometryDrawing()
                {
                    Pen = new(Brushes.Black, 0.05),
                    Brush = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    Geometry = new RectangleGeometry(new Rect(0, 0, 45, 20))
                }
            };

            return brush;
        }    
    }
}
