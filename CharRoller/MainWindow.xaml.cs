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

        private string[] CompareA(string[] a, string[] b)
        {
            string[] buffer = new string[] {};
            foreach (string s in a)
            {
                foreach (string s2 in b)
                {
                    if (s == s2)
                    {
                        break;
                    }
                }
            }
            return buffer;
        }

        private void RePickAction()
        {
            nameBox.Content = nList[Randomizer(0, nList.Length)];
            HistoryBox.Text = HistoryBox.Text + nameBox.Content + "\n";
            #pragma warning disable CS8629 // 可为 null 的值类型可为 null。
            try
            {
                if ((bool)noRepeatCheck.IsChecked)
                {
                    nList = (string[])File.ReadAllLines("list.txt").Except(HistoryBox.Text.Split('\n'));
                }
            }
            catch { }
            #pragma warning restore CS8629
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
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryBox.Clear();
        }
    }
}
