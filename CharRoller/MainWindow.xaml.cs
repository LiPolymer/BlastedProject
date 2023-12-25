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

namespace CharRoller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cfgCard.Visibility = Visibility.Collapsed;
            ResetButton.Visibility = Visibility.Collapsed;
            ClearButton.Visibility = Visibility.Collapsed;
            Initialize();
        }
        //----------------Variables---------------
        string[] nList = new string[] {};
        //--------------LogicProcessor------------
        private void Initialize()
        {
            if (File.Exists("list.txt"))
            {
                nList = File.ReadAllLines("list.txt");
                if (nList.Length > 0 )
                {
                    nameBox.Content = ":) " + nList.Length.ToString() +  "个项已载入";
                }
                else
                {
                    nameBox.Content = ":( 空的列表";
                    nameBox.IsEnabled = false;
                    refreshBtn.IsEnabled = false;
                }
            }
            else
            {
                File.WriteAllText("list.txt","");
                Initialize();
            }
        }

        Random ran = new Random();
        private int Randomizer(int from, int to)
        {
            return ran.Next(from, to);
        }

        private string[] ExProcessor(string[] a, string[] b)
        {
            return a.Except(b).ToArray();
        }

        private void RePickAction()
        {
            #pragma warning disable CS8629 // 可为 null 的值类型可为 null。
            if ((bool)noRepeatCheck.IsChecked)
            {
                nList = ExProcessor(nList, HistoryBox.Text.Split('\n'));
            }
            else
            {
                nList = File.ReadAllLines("list.txt");
            }
            #pragma warning restore CS8629
            if (nList.Length > 0 )
            {
                nameBox.Content = nList[Randomizer(0, nList.Length)];
                HistoryBox.Text = HistoryBox.Text + nameBox.Content + "\n";
            }
            else
            {
                ResetButton.Visibility = Visibility.Visible;
                refreshBtn.IsEnabled = false;
                nameBox.Content = "请重置记录";
            }
        }
        //--------------ViewProcessor------------

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.WindowState = WindowState.Minimized;
        }

        Thickness wcNt = new Thickness(0, 0, 0, 0);
        Thickness wcMt = new Thickness(0, 8, 8, 0);
        private void FullButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                mainWindow.WindowState = WindowState.Normal;
                windowCtrlGrid.Margin = wcNt;
                nameBox.FontSize = 36;
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                windowCtrlGrid.Margin = wcMt;
                nameBox.FontSize = 120;
            }

        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (cfgCard.Visibility == Visibility.Visible)
            {
                cfgCard.Visibility = Visibility.Collapsed;
            }
            else
            {
                cfgCard.Visibility = Visibility.Visible;
            }
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RePickAction();
            ClearButton.Visibility = Visibility.Visible;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryBox.Clear();
            nList = File.ReadAllLines("list.txt");
            nameBox.Content = ":D";
            ResetButton.Visibility = Visibility.Collapsed;
            refreshBtn.IsEnabled = true;
            ClearButton.Visibility = Visibility.Collapsed;
        }
    }
}
