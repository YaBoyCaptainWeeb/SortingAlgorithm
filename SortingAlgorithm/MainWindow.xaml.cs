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
using SortingAlgorithm.Charts;
using System.Threading;


namespace SortingAlgorithm
{
    public partial class MainWindow : Window
    {
        int index, writtenDownAlready = 0;
        public int[] green = new int[4] { 73, 245, 39, 1 };
        public int[] orange = new int[4] { 188, 212, 36, 1};
        List<string> toSortArray = new List<string>();
        public List<List<int>> History = new List<List<int>>();
        public MainWindow()
        {
            InitializeComponent();
        }
        // Сортировка прямыми вставками с бинарным поиском
        private void insertionSorting(object sender, EventArgs Click)
        {
            // перевод интерфейса в состояние "слайдера"
            if (StartBtn.Visibility == Visibility.Visible)
            {
                StartBtn.Visibility = Visibility.Collapsed;
                BackBtn.IsEnabled = true;
                NextBtn.IsEnabled = true;
                BackBtn.Visibility = Visibility.Visible;
                NextBtn.Visibility = Visibility.Visible;
                StartBtn.IsEnabled = false;
                EndBtn.IsEnabled = true;
            }
            // Вызов функции конвертации символьного массива в численный тип данных
            int[] data = ConvertData(toSortArray);
            int loc, j, selected;
            for (int i = 1; i < data.Length; i++)
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
                }
            History.Add(new List<int>());
            foreach (var item in data)
            {
                History[data.Length-1].Add(item);
            }
            MessageBoxResult res = MessageBox.Show("Хотите записать результат в файл заранее?", "Запрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                WriteDownResult(data);
                writtenDownAlready = 1;
            }
        }
        private int BinarySearch(int[] data, int item, int low, int high)
        // Функция бинарного поиска в сортировке прямыми вставками            
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
        private void BackWards(object sender, EventArgs Click) // Кнопка перехода назад в демонстрации
        {
            int[] toUpdateData = new int[History.Count];
            if (index > 0)
            {
                for (int i = 0; i < toUpdateData.Length; i++)
                {
                    toUpdateData[i] = History[index - 1][i];
                }
                index--;
                GridForChart.Children.OfType<Canvas>().ToList().ForEach(p => GridForChart.Children.Remove(p));
                Chart chart = new Visualization();
                GridForChart.Children.Add(chart.ChartBackground);
                GridForChart.UpdateLayout();
                CreateChart(chart, toUpdateData);
            } else
            {          
                GridForChart.Children.OfType<Canvas>().ToList().ForEach(p => GridForChart.Children.Remove(p));
                Chart chart = new Visualization();
                GridForChart.Children.Add(chart.ChartBackground);
                GridForChart.UpdateLayout();
                foreach (var item in History[0])
                {
                    chart.AddValue(item);
                }
                MessageBoxResult res = MessageBox.Show("График находится на начальном этапе сортировки"
                    , "Конец"
                    , MessageBoxButton.OK
                    , MessageBoxImage.Hand);
            }
            
        }
        private void Forward(object sender, EventArgs Click) // Кнопка перехода вперед по демонстрации
        {
            int[] toUpdateData = new int[History.Count];
            if (index + 1 < History.Count)
            {
                for (int i = 0; i < toUpdateData.Length; i++)
                {
                    toUpdateData[i] = History[index + 1][i];
                }
                index++;
                GridForChart.Children.Clear();
                Chart chart = new Visualization();
                GridForChart.Children.Add(chart.ChartBackground);
                GridForChart.UpdateLayout();
                CreateChart(chart, toUpdateData);
            } else
            {
                MessageBoxResult res = MessageBox.Show("График находится на конечном этапе сортировки"
                    + "\n" + "Хотите закончить демонстрацию?"
                    , "Конец"
                    , MessageBoxButton.YesNo
                    , MessageBoxImage.Hand);
                if (res == MessageBoxResult.Yes && writtenDownAlready == 0)
                {
                    MessageBoxResult res1 = MessageBox.Show("Хотите записать результат в файл?"
                        , "Запрос"
                        , MessageBoxButton.YesNo
                        , MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        for (int i = 0; i < toUpdateData.Length; i++)
                        {
                            toUpdateData[i] = History[index][i];
                        }
                        WriteDownResult(toUpdateData);
                        ResetData();
                    }
                    else
                    {
                        ResetData();
                    }
                } else if (res == MessageBoxResult.Yes && writtenDownAlready == 1)
                {
                    ResetData();
                }
            } 
        }
        private void EndDemonstration(object sender, EventArgs e) // Завершение демонстрации
        {
            int[] toUpdateData = new int[History.Count];
            MessageBoxResult res = MessageBox.Show("Хотите закончить демонстрацию?"
                   , "Запрос"
                   , MessageBoxButton.YesNo
                   , MessageBoxImage.Hand);
            if (res == MessageBoxResult.Yes && writtenDownAlready == 0)
            {
                MessageBoxResult res1 = MessageBox.Show("Хотите записать результат в файл?"
                    , "Запрос"
                    , MessageBoxButton.YesNo
                    , MessageBoxImage.Question);
                if (res1 == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < toUpdateData.Length; i++)
                    {
                        toUpdateData[i] = History[index][i];
                    }
                    WriteDownResult(toUpdateData);
                    ResetData();
                } else
                {
                    ResetData();
                }
            }
            else
            {
                ResetData();
            }
        }
        public void ResetData() // Перевод интерфейса в состояние по умолчанию
        {
            StartBtn.IsEnabled = false;
            StartBtn.Visibility = Visibility.Visible;
            BackBtn.Visibility = Visibility.Collapsed;
            NextBtn.Visibility = Visibility.Collapsed;
            EndBtn.IsEnabled = false;
            LoadedData.Text = "";
            index = 0;
            History.Clear();
            GridForChart.Children.Clear();
        }
        private void WriteDownResult(int[] data) // Запись результата в файл
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
            }            
        }
        // Функция открытия выбранного файла с набором чисел
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
                StartBtn.IsEnabled = true;
                StartBtn.Visibility = Visibility.Visible;
                BackBtn.Visibility = Visibility.Collapsed;
                NextBtn.Visibility = Visibility.Collapsed;
                // Обращение к визуализации
                GridForChart.Children.Clear();
                Chart chart = new Visualization();
                GridForChart.Children.Add(chart.ChartBackground);
                GridForChart.UpdateLayout();
                CreateChart(chart, ConvertData(toSortArray));
            }
        }
        // Создания объекта типа chart и добавление его на диаграмму
        private void CreateChart(Chart chart, int[] List)
        {
            chart.Clear();
            List<int> list = new List<int>();
            int[] comparable = new int[History.Count];
            if (History.Count != 0)
            {
                for (int i = 0; i != List.Length; i++)
                {
                    list.Add(List[i]);
                    comparable[i] = History[History.Count - 1][i];
                }
                for (int i = 0; i != list.Count; i++)
                {
                    chart.AddValue(list[i], comparable);
                }
            } else
            {
                for (int i = 0; i != List.Length; i++)
                {
                    list.Add(List[i]);
                }
                for (int i = 0; i != list.Count; i++)
                {
                    chart.AddValue(list[i]);
                }
            }
        }
        // Конвертация символьного массива в тип int 
        public int[] ConvertData(List<string> toConvertArr)
        {
            int[] data = new int[toSortArray.Count];
            for (int i = 0; i < data.Length; i++)
            {
                if (toConvertArr[i] != "\n" && toConvertArr[i] != " ")
                {
                    data[i] = Convert.ToInt32(toConvertArr[i]);
                }
            }
            return data;
        }
        // справка
        private void ShowInfo(object sender, EventArgs Click)
        {
            MessageBox.Show("Демонстрационная программа метода сортировки " +
                "'прямыми вставками с бинарным поиском'." +
                "Пользователь должен выбрать файл с набором чисел, после чего произойдет сортировка, " +
                "и пользователь сможет нажимать кнопки 'Назад' и 'Вперед', посматривая шаги сортировки алгоритма." + "\n \n"
                + "Автор программы: " + "\n \n" + "Цымбалов В.А. 20-КБ(с)РЗПО-1"
                , "Справочная информация"
                , MessageBoxButton.OK
                , MessageBoxImage.Asterisk);
        }
    }
}