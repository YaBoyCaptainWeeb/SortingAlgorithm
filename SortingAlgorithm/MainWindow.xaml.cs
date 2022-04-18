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
using static SortingAlgorithm.Visualization;

namespace SortingAlgorithm
{
    public partial class MainWindow : Window
    {
        List<string> toSortArray = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void insertionSorting(object sender, EventArgs Click)
        {
            int[] data = new int[toSortArray.Count];
            int i;
            //MessageBox.Show(Convert.ToString(toSortArray.Count));
            for (i = 0; i < data.Length; i++)
            {
                if (toSortArray[i] != " ")
                {
                    data[i] = Convert.ToInt32(toSortArray[i]);
                    MessageBox.Show(Convert.ToString(data[i]));
                }
            }
            FirstScene(data);
            //MessageBox.Show(Convert.ToString(data.Length));
            int loc, j, selected;
            for (i = 1; i < data.Length; i++)
            {
                j = i - 1;
                selected = data[i];

                loc = BinarySearch(data, selected, 0, j); // вызов бинарного поиска для текущего элемента в массиве

                while (j >= loc)
                {
                    data[j + 1] = data[j];
                    j--;
                }
                data[loc] = selected;
            }
            WriteDownResult(data);
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
        private void WriteDownResult(int[] data)
        {
            string[] text = new string[data.Length];
            SaveFileDialog newFile = new SaveFileDialog();
            newFile.Title = "Сохранить файл как...";
            newFile.Filter = "Текстовый файл (*.txt) | *.txt";
            newFile.ShowDialog();
            for (int i = 0; i != data.Length; i++)
            {
                text[i] = Convert.ToString(data[i]);
            }
            if (newFile.FileName != null)
            {
                File.WriteAllLines(newFile.FileName, text);
            }
        }
        private void OpenFile(object sender, EventArgs Click)
        {
            string textFromFile, singleLine = "";
            toSortArray.Clear();
            OpenFileDialog Conductor = new OpenFileDialog();
            Conductor.Title = "Выберите файл с данными";
            Conductor.Filter = "Текстовый файл (*.txt) | *.txt";
            Conductor.ShowDialog();
            textFromFile = File.ReadAllText(Conductor.FileName, Encoding.UTF8);
            LoadedData.Text = textFromFile;
            // int j = 0;
            for (int i = 0; i != textFromFile.Length; i++)
            {
                if (textFromFile[i] == '\n' || textFromFile[i] == '\0')
                {
                    toSortArray.Add(singleLine);
                    //MessageBox.Show(toSortArray[j]);
                    //j++;
                    singleLine = "";
                    continue;
                }
                singleLine += textFromFile[i];
            }
        }
        private void ShowInfo(object sender, EventArgs Click)
        {
            MessageBox.Show("КОНТЕНТ ДОРАБОТАТЬ", "Справочная информация", MessageBoxButton.OK);
        }
    }
}