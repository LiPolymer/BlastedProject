using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        /// 日期点击
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

        /// <summary>
        /// 时间差异计算
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int DiffDays(DateTime startTime, DateTime endTime)
        {
            TimeSpan daysSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
            return daysSpan.Days;
        }

        public TimeSpan DiffAccu(DateTime startTime, DateTime endTime)
        {
            TimeSpan daysSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
            return daysSpan;
        }

        /// <summary>
        /// 窗口加载动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loader();
        }

        string[] cfg;
        private void Loader()
        {
            if (!File.Exists(".\\datecfg.txt"))
            {
                //MessageBox.Show("配置文件丢失,无法启动力!", "悲 | BlastedDateCountdown", MessageBoxButton.OK, MessageBoxImage.Error);
                //Environment.Exit(0);
                File.WriteAllText(".\\datecfg.txt", "d,2025/1/1,New Year!,10,10,0.9");
                Loader();
            }
            else
            {
                cfgBox.Text = File.ReadAllText(".\\datecfg.txt");
                cfgCard.Visibility = Visibility.Collapsed;
                cfg = cfgBox.Text.Split(',');
                if (cfg[0] == "d")
                {
                    init();
                }
                else
                {
                    Thread refreshT = new Thread(cycle);
                    refreshT.Start();
                }
            }
        }

        private void cycle()
        {
            while (true)
            {
                mainWindow.Dispatcher.Invoke(() => { cycletick(); });
                Thread.Sleep(100);
            }
        }

        private void cycletick()
        {
            string timeStr = cfg[1];
            string timeName = cfg[2];
            mainWindow.Top = Convert.ToInt32(cfg[3]);
            mainWindow.Left = Convert.ToInt32(cfg[4]);
            mainCard.Opacity = Convert.ToDouble(cfg[5]);
            DateTime targetTime = Convert.ToDateTime(timeStr);
            TimeSpan diff = DiffAccu(DateTime.Now.ToLocalTime(), targetTime);
            smallBox.Text = timeName;
            if (cfg[0] == "dh")
            {
                mainWindow.Width = 250;
                mainCard.Width = 200;
                bigBox.Text = "还有" + Convert.ToString(diff.Days) + "天" + Convert.ToString(diff.Hours) + "时";
            }

            else if (cfg[0] == "dhm")
            {
                mainWindow.Width = 260;
                mainCard.Width = 250;
                bigBox.Text = "还有" + Convert.ToString(diff.Days) + "天"
                    + Convert.ToString(diff.Hours) + "时" 
                    + Convert.ToString(diff.Minutes) + "分";
            }

            else if (cfg[0] == "dhms")
            {
                mainWindow.Width = 300;
                mainCard.Width = 290;
                bigBox.Text = "还有" + Convert.ToString(diff.Days) + "天"
                    + Convert.ToString(diff.Hours) + "时"
                    + Convert.ToString(diff.Minutes) + "分"
                    + Convert.ToString(diff.Seconds) + "秒";
            }
        }

        /// <summary>
        /// 控件内容初始化
        /// </summary>
        private void init()
        {
            try
            {
                string cfgstr = File.ReadAllText(".\\datecfg.txt");
                string[] cfg = cfgstr.Split(',');
                string timeStr = cfg[1];
                string timeName = cfg[2];
                mainWindow.Top = Convert.ToInt32(cfg[3]);
                mainWindow.Left = Convert.ToInt32(cfg[4]);
                mainCard.Opacity = Convert.ToDouble(cfg[5]);
                DateTime targetTime = Convert.ToDateTime(timeStr);
                bigBox.Text ="还有" + Convert.ToString(DiffDays(DateTime.Now.ToLocalTime(), targetTime)) + "天";
                smallBox.Text = timeName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置文件炸力!\r\n错误信息\r\n" + ex, "悲 | BlastedDateCountdown", MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 重载按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadBtn_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(".\\datecfg.txt", cfgBox.Text);
            Loader();
        }
    }
}
