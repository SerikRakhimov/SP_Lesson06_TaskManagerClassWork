using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;

namespace TaskManagerClassWork
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();
        public App()
        {
            nIcon.Icon = new Icon(@"MyIcon.ico");
            nIcon.Visible = true;
            nIcon.ShowBalloonTip(5000, "TaskManagerClassWork", "Диспетчер задач (классная работа)", System.Windows.Forms.ToolTipIcon.Info);
            nIcon.Click += nIcon_Click;
        }

        void nIcon_Click(object sender, EventArgs e)
        {
            //events comes here
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.WindowState = WindowState.Normal;
        }
    }
}
