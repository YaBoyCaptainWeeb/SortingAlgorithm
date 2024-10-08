﻿using Microsoft.Win32;
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
using SortingAlgorithm.Charts;
using System.Threading;


namespace SortingAlgorithm
{
    public partial class MainWindow : Window
    {
        int i = 1;
        int[] data;
        int loc, j, selected;
        List<string> toSortArray = new List<string>();
        List<List<int>> History = new List<List<int>>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void insertionSorting(object sender, EventArgs Click)
        {
            if (StartBtn.Visibility == Visibility.Visible)
            {
                StartBtn.Visibility = Visibility.Collapsed;
                BackBtn.IsEnabled = true;
                NextBtn.IsEnabled = true;
                BackBtn.Visibility = Visibility.Visible;
                NextBtn.Visibility = Visibility.Visible;
                StartBtn.IsEnabled = false;
                NextBtn.Click += insertionSorting;
            }
                if (i < data.Length)
                {
                History.Add(new List<int>());
                foreach (var item in data)
                {
                    History[i-1].Add(item);
                }
                j = i - 1;
                    selected = data[i];

                    loc = BinarySearch(data, selected, 0, j); // вызов бинарного поиска для текущего элемента в массиве

                    while (j >= loc)
                    {
                        data[j + 1] = data[j];
                        j--;
                    }
                    data[loc] = selected;
                    i++;
                    GridForChart.Children.OfType<Canvas>().ToList().ForEach(p => GridForChart.Children.Remove(p));
                    Chart chart = new Visualization();
                    GridForChart.Children.Add(chart.ChartBackground);
                    GridForChart.UpdateLayout();
                    CreateChart(chart, data);
                }
                else
                {
                History.Add(new List<int>());
                foreach (var item in data)
                {
                    History[i - 1].Add(item);
                }
                    WriteDownResult(data);
                }
        }
        private int BinarySearch(int[] data, int item, int low, int high)
        {
            if (high <= low)
            {
                return (item > data[low]) ? (low + 1) : low;
            }
            int mid = (low + high) / 2;
            if (item == data[mid])
            {
                return mid + 1;
            }
            if (item > data[mid])
            {
                return BinarySearch(data, item, mid + 1, high);
            }
            return BinarySearch(data, item, low, mid - 1);

        }
        private void BackWards(object sender, EventArgs Click)
        {
            int[] toSendData = new int[History[i-2].Count];
            for (int j = 0; j != History[i - 2].Count; j++)
            {
                toSendData[j] = History[i - 2][j];
            }
            GridForChart.Children.OfType<Canvas>().ToList().ForEach(p => GridForChart.Children.Remove(p));
            Chart chart = new Visualization();
            GridForChart.Children.Add(chart.ChartBackground);
            GridForChart.UpdateLayout();
            CreateChart(chart, toSendData);
        }
        private void Forward(object sender, EventArgs Click)
        {

        }
        private void WriteDownResult(int[] data)
        {
            string[] text = new string[data.Length];
            SaveFileDialog newFile = new SaveFileDialog();
            newFile.Title = "Сохранить файл как...";
            newFile.Filter = "Текстовый файл (*.txt) | *.txt";
            if (newFile.ShowDialog() == true)
            {
                for (int i = 0; i != data.Length; i++)
                {
                    text[i] = Convert.ToString(data[i]);
                }
                File.WriteAllLines(newFile.FileName, text);
                StartBtn.IsEnabled = true;
                StartBtn.Visibility = Visibility.Visible;
                BackBtn.Visibility = Visibility.Collapsed;
                NextBtn.Visibility = Visibility.Collapsed;
                BackBtn.IsEnabled = false;
                NextBtn.IsEnabled = false;
            }            
        }
        private void OpenFile(object sender, EventArgs Click)
        {
            string textFromFile, singleLine = "";
            toSortArray.Clear();
            OpenFileDialog Conductor = new OpenFileDialog();
            Conductor.Title = "Выберите файл с данными";
            Conductor.Filter = "Текстовый файл (*.txt) | *.txt";
            if (Conductor.ShowDialog() == true)
            {
                textFromFile = File.ReadAllText(Conductor.FileName, Encoding.UTF8);
                LoadedData.Text = textFromFile;
                for (int i = 0; i != textFromFile.Length; i++)
                {
                    if (textFromFile[i] == '\n' || textFromFile[i] == '\0')
                    {
                        toSortArray.Add(singleLine);
                        singleLine = "";
                        continue;
                    }
                    singleLine += textFromFile[i];
                }
                data = ConvertData(toSortArray);
                StartBtn.IsEnabled = true;
                // Обращение к визуализации
                GridForChart.Children.OfType<Canvas>().ToList().ForEach(p => GridForChart.Children.Remove(p));
                Chart chart = new Visualization();
                GridForChart.Children.Add(chart.ChartBackground);
                GridForChart.UpdateLayout();
                CreateChart(chart, ConvertData(toSortArray));
            }
        }
        private static void CreateChart(Chart chart, int[] List)
        {
            chart.Clear();
            List<int> list = new List<int>();
            for (int i = 0; i != List.Length; i++)
            {
                list.Add(List[i]);
                //MessageBox.Show(Convert.ToString(List[i]), "Визуализация");
            }
            for (int i = 0; i != list.Count; i++)
            {
                chart.AddValue(list[i]);
            }
        }
        public int[] ConvertData(List<string> toConvertArr)
        {
            int[] data = new int[toSortArray.Count];
            for (int i = 0; i < data.Length; i++)
            {
                if (toConvertArr[i] != "\n" && toConvertArr[i] != " ")
                {
                    data[i] = Convert.ToInt32(toConvertArr[i]);
                    //MessageBox.Show(Convert.ToString(data[i]));
                }
            }
            return data;
        }
        private void ShowInfo(object sender, EventArgs Click)
        {
            MessageBox.Show("КОНТЕНТ ДОРАБОТАТЬ", "Справочная информация", MessageBoxButton.OK);
        }
    }
}