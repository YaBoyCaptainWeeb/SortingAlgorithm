using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SortingAlgorithm;

namespace SortingAlgorithm.Charts
{
    internal class Visualization : Chart
    {
        public int[] green = new int[3] { 73, 245, 39 };
        public int[] orange = new int[3] { 252, 194, 3 };
        private double gap = 5;
        public override void Clear() => ChartBackground.Children.Clear();
        // Функция добавления объекта на диаграмму
        public override void AddValue(int data, int[] comparable)
        {
            List<double> listValues = ChartBackground.Children.OfType<Rectangle>().Select(p => (double)p.Tag).ToList();
            listValues.Add(data);
            double WidthBar = (WidthChart - ((listValues.Count - 1) * gap)) / listValues.Count;
            double MaxValue = listValues.Max();
            double denominator = MaxValue / HeightChart;
            
            Clear();
            for (int i = 0; i < listValues.Count; i++)
            {
                int count = ChartBackground.Children.OfType<Rectangle>().Count();
                double heightPoint = listValues[i] / denominator;
                if (heightPoint < 3)
                {
                    heightPoint = 3;
                }
                double x = (count * (WidthBar + gap)) + (ChartBackground.ActualWidth - WidthChart) / 2;
                if (listValues[i] == comparable[i])
                {
                    Rectangle bar = CreateBar(x, heightPoint, WidthBar, listValues[i], green);
                    _ = ChartBackground.Children.Add(bar);
                    Label title = CreateTitle(x, bar.Height, WidthBar, listValues[i]);
                    _ = ChartBackground.Children.Add(title);
                } else
                {
                    Rectangle bar = CreateBar(x, heightPoint, WidthBar, listValues[i], orange);
                    _ = ChartBackground.Children.Add(bar);
                    Label title = CreateTitle(x, bar.Height, WidthBar, listValues[i]);
                    _ = ChartBackground.Children.Add(title);
                }
            }
        }
        public override void AddValue(int data)
        {
            List<double> listValues = ChartBackground.Children.OfType<Rectangle>().Select(p => (double)p.Tag).ToList();
            listValues.Add(data);
            double WidthBar = (WidthChart - ((listValues.Count - 1) * gap)) / listValues.Count;
            double MaxValue = listValues.Max();
            double denominator = MaxValue / HeightChart;
            Clear();
            foreach (double val in listValues)
            {
                int count = ChartBackground.Children.OfType<Rectangle>().Count();
                double heightPoint = val / denominator;
                if (heightPoint < 3)
                {
                    heightPoint = 3;
                }
                double x = (count * (WidthBar + gap)) + (ChartBackground.ActualWidth - WidthChart) / 2;

                    Rectangle bar = CreateBar(x, heightPoint, WidthBar, val, orange);
                    _ = ChartBackground.Children.Add(bar);
                    Label title = CreateTitle(x, bar.Height, WidthBar, val);
                    _ = ChartBackground.Children.Add(title);
            }
        }
        /// <summary>
        /// Создание полосы графика
        /// </summary>
        /// <param name="x">x координата</param>
        /// <param name="height">высота</param>
        /// <param name="width">ширина</param>
        /// <param name="value">абсолютное значение</param>
        /// <returns></returns>
        private Rectangle CreateBar(double x, double height, double width, double value, int[] color)
        {
            Rectangle bar = new()
            {
                Stroke = Brushes.Black,
                Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(color[0]), Convert.ToByte(color[1]), Convert.ToByte(color[2])))
                ,
                Height = height,
                Width = width,
                StrokeThickness = 0.5,
                Tag = value
            };
            Canvas.SetLeft(bar, x);
            Canvas.SetBottom(bar, 0);

            return bar;
        }
        /// <summary>
        /// Создание текстовой надписи над полосой графика.
        /// </summary>
        /// <param name="x">x координата</param>
        /// <param name="y">y координата</param>
        /// <param name="width">ширина поля надписи</param>
        /// <param name="value">абсолютное значение выводится как текст</param>
        /// <returns></returns>
        private Label CreateTitle(double x, double y, double width, double value)
        {
            Label title = new()
            {
                Content = value,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Width = width,
                Padding = new Thickness(0, 0, 0, 10)
            };

            Canvas.SetLeft(title, x);
            Canvas.SetBottom(title, y);

            return title;
        }
    }
}


