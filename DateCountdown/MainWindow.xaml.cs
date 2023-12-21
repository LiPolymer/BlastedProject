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

namespace WpfPSM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bigBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(cfgCard.Visibility == Visibility.Visible)
            {
                cfgCard.Visibility = Visibility.Collapsed;
            }
            else
            {
                cfgCard.Visibility = Visibility.Visible;
            }
        }

        public int DiffDays(DateTime startTime, DateTime endTime)
        {
            TimeSpan daysSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
            return daysSpan.Days;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(".\\datecfg.txt"))
            {
                MessageBox.Show("配置文件丢失,无法启动力!", "悲 | DateBackCounter17", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            else
            {
                cfgBox.Text = File.ReadAllText(".\\datecfg.txt");
                cfgCard.Visibility = Visibility.Collapsed;
                init();
            }
        }

        private void init()
        {
            try
            {
                string cfgstr = File.ReadAllText(".\\datecfg.txt");
                string[] cfg = cfgstr.Split(',');
                string timeStr = cfg[0];
                string timeName = cfg[1];
                mainWindow.Top = Convert.ToInt32(cfg[2]);
                mainWindow.Left = Convert.ToInt32(cfg[3]);
                mainCard.Opacity = Convert.ToDouble(cfg[4]);
                DateTime targetTime = Convert.ToDateTime(timeStr);
                bigBox.Text ="还有" + Convert.ToString(DiffDays(DateTime.Now.ToLocalTime(), targetTime)) + "天";
                smallBox.Text = timeName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置文件炸力!\r\n错误信息\r\n" + ex,"悲 | DateBackCounter17", MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(0);
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void reloadBtn_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(".\\datecfg.txt", cfgBox.Text);
            init();
        }
    }
}
