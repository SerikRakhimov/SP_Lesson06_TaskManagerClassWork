using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerClassWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void MyAsyncDelegate(object obj);
        System.Threading.Timer timer;

        public MainWindow()
        {
            InitializeComponent();
            dpDate.SelectedDate = DateTime.Today;
            rbDownloadFile_Click(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new TaskManagerContext();
            var myTasks = context.MyTasks.ToList();
            foreach (var myTask in myTasks)
            {
                EventRun(myTask);
            };
        }

        // Сворачивание программы в трей при закрытии программы - http://qaru.site/questions/799733/can-i-use-notifyicon-in-wpf
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void EventRun(MyTask myTask)
        {
            TimeSpan timeRepeat;
            DateTime datetimeMain = DateTime.Now;
            if (myTask.DateTimeStart >= datetimeMain)
            {
                TimeSpan interval = myTask.DateTimeStart - datetimeMain;

                if (myTask.Periodcity == MyPeriodcity.OneTime)
                {
                    timeRepeat = TimeSpan.FromTicks(0);
                }
                else
                {
                    timeRepeat = TimeSpan.FromMinutes(1);
                }

                switch (myTask.Event)
                {
                    case MyEvent.DownloadFile:
                        timer = new System.Threading.Timer(DownloadFileAsync, myTask, interval, timeRepeat);
                        break;
                    case MyEvent.MoveFolder:
                        timer = new System.Threading.Timer(MoveFolderAsync, myTask, interval, timeRepeat);
                        break;
                    case MyEvent.SendEmail:
                        timer = new System.Threading.Timer(SendEmailAsync, myTask, interval, timeRepeat);
                        break;
                }
            }
        }
        
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            int chs, min, sec;
            MyEvent myEvent;
            MyPeriodcity myPeriodcity;
            string parametr1, parametr2;
            chs = 0;
            min = 0;
            sec = 0;
            myEvent = MyEvent.DownloadFile;
            myPeriodcity = MyPeriodcity.OneTime;
            try
            {
                chs = int.Parse(txtHours.Text);
                if (!((0 <= chs) && (chs <= 23)))
                {
                    System.Windows.Forms.MessageBox.Show("Часы нужно ввести от 0 до 23. Повторите ввод.");
                    return;
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Введите часы от 0 до 23. Повторите ввод.");
                return;
            }
            try
            {
                min = int.Parse(txtMinuts.Text);
                if (!((0 <= min) && (min <= 59)))
                {
                    System.Windows.Forms.MessageBox.Show("Минуты нужно ввести от 0 до 59. Повторите ввод.");
                    return;
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Введите минуты от 0 до 59. Повторите ввод.");
                return;
            }
            try
            {
                sec = int.Parse(txtSeconds.Text);
                if (!((0 <= sec) && (sec <= 59)))
                {
                    System.Windows.Forms.MessageBox.Show("Секунды нужно ввести от 0 до 59. Повторите ввод.");
                    return;
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Введите секунды от 0 до 59. Повторите ввод.");
                return;
            }
            if (dpDate.SelectedDate.ToString() == "")
            {
                System.Windows.Forms.MessageBox.Show("Введите дату.");
                return;
            }
            parametr1 = "";
            parametr2 = "";
            if (rbDownloadFile.IsChecked == true)
            {
                myEvent = MyEvent.DownloadFile;
                parametr1 = txtFromFile.Text;
                parametr2 = txtToFile.Text;
            }
            else if (rbMoveFolder.IsChecked == true)
            {
                myEvent = MyEvent.MoveFolder;
                parametr1 = txtFromFolder.Text;
                parametr2 = txtToFolder.Text;
            }
            else if (rbSendEMAil.IsChecked == true)
            {
                myEvent = MyEvent.SendEmail;
                parametr1 = txtWhom.Text;
                parametr2 = txtText.Text;
            }

            if (rbOneTime.IsChecked == true)
            {
                myPeriodcity = MyPeriodcity.OneTime;
            }
            else if (rbEveryDay.IsChecked == true)
            {
                myPeriodcity = MyPeriodcity.EveryDay;
            }
            else if (rbEveryMonth.IsChecked == true)
            {
                myPeriodcity = MyPeriodcity.EveryMonth;
            }
            else if (rbEveryYear.IsChecked == true)
            {
                myPeriodcity = MyPeriodcity.EveryYear;
            }
            
            DateTime dateInput = (DateTime) dpDate.SelectedDate;
            DateTime datetimeEvent = new DateTime(dateInput.Year, dateInput.Month, dateInput.Day, chs, min, sec);
            var taskManagerContext = new TaskManagerContext();
            MyTask myTask = new MyTask
            {
                DateTimeStart = datetimeEvent,
                Event = myEvent,
                Periodcity = myPeriodcity,
                Parameter1 = parametr1,
                Parameter2 = parametr2
            };
            taskManagerContext.MyTasks.Add(myTask);
            taskManagerContext.SaveChanges();
            EventRun(myTask);
            System.Windows.Forms.MessageBox.Show("Событие учтено.");
        }

        private void rbDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            spDownloadFile.Visibility = Visibility.Visible;
            spMoveFolder.Visibility = Visibility.Hidden;
            spSendEmail.Visibility = Visibility.Hidden;
        }

        private void rbMoveFolder_Click(object sender, RoutedEventArgs e)
        {
            spDownloadFile.Visibility = Visibility.Hidden;
            spMoveFolder.Visibility = Visibility.Visible;
            spSendEmail.Visibility = Visibility.Hidden;
        }

        private void rbSendEMAil_Click(object sender, RoutedEventArgs e)
        {
            spDownloadFile.Visibility = Visibility.Hidden;
            spMoveFolder.Visibility = Visibility.Hidden;
            spSendEmail.Visibility = Visibility.Visible;

        }
        private string ReturnFolder()
        {
            string resultFolder = "";
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult dialogResult = fbd.ShowDialog();

                if (dialogResult == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    resultFolder = fbd.SelectedPath;
                }
            }
            return resultFolder;
        }
        private void btToFile_Click(object sender, RoutedEventArgs e)
        {
            string resultFolder = ReturnFolder();
            if (resultFolder != "")
            {
                txtToFile.Text = resultFolder;
            }
        }

        private void btToFolder_Click(object sender, RoutedEventArgs e)
        {
            string resultFolder = ReturnFolder();
            if (resultFolder != "")
            {
                txtToFolder.Text = resultFolder;
            }
        }

        private void btFromFolder_Click(object sender, RoutedEventArgs e)
        {
            string resultFolder = ReturnFolder();
            if (resultFolder != "")
            {
                txtFromFolder.Text = resultFolder;
            }
        }

        private bool EventStart(MyTask myTask)
        {
            bool result;
            DateTime dateNow = DateTime.Now;
            DateTime dateEvent = myTask.DateTimeStart;

            result = false;

            switch (myTask.Periodcity)
            {
                case MyPeriodcity.OneTime:
                    result = true;
                    break;
                case MyPeriodcity.EveryDay:
                    result = (dateEvent.Hour == dateNow.Hour) && (dateEvent.Minute == dateNow.Minute);
                    break;
                case MyPeriodcity.EveryMonth:
                    result = (dateEvent.Hour == dateNow.Hour) && (dateEvent.Minute == dateNow.Minute)
                             && (dateEvent.Day == dateNow.Day);
                    break;
                case MyPeriodcity.EveryYear:
                    result = (dateEvent.Hour == dateNow.Hour) && (dateEvent.Minute == dateNow.Minute)
                             && (dateEvent.Day == dateNow.Day) && (dateEvent.Month == dateNow.Month);
                    break;
            }

            return result;

        }
        private void DownloadFileAsync(object obj)
        {
            if (EventStart((MyTask) obj))
            {
                MyAsyncDelegate asyncDelegate = new MyAsyncDelegate(DownloadFile);
                var asyncResult = asyncDelegate.BeginInvoke(obj, null, null);
            }
        }

        private void DownloadFile(object obj)
        {
            WebClient client = new WebClient();
            MyTask myTask = (MyTask) obj;
            string fileFrom = myTask.Parameter1;
            string fileTo = myTask.Parameter2 + "\\" + System.IO.Path.GetFileName(fileFrom);
            try
            {
                client.DownloadFile(fileFrom, fileTo);
            }
            catch (Exception exception)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при скачивании файла '" + fileFrom + "'" +
                                                     (char)13 + " в '" + fileTo + "':" + (char)13 + exception.Message + ".");
            }
        }

        private void MoveFolderAsync(object obj)
        {
            if (EventStart((MyTask)obj))
            {
                MyAsyncDelegate asyncDelegate = new MyAsyncDelegate(MoveFolder);
                var asyncResult = asyncDelegate.BeginInvoke(obj, null, null);
            }
        }

        private void MoveFolder(object obj)
        {
            MyTask myTask = (MyTask)obj;
            string src = myTask.Parameter1;
            string dest = myTask.Parameter2;
            try
            {
                DirectoryInfo destDir = new DirectoryInfo(dest);
                if (destDir.Exists)
                    destDir.Delete(true);
                new DirectoryInfo(src).MoveTo(dest);
            }
            catch(Exception exception)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка при перемещении папки '" + src + "'" +
                                                    (char)13 + " в '" + dest + "':" + (char)13 + exception.Message + ".");
            }
        }

        private void SendEmailAsync(object obj)
        {
            if (EventStart((MyTask)obj))
            {
                MyAsyncDelegate asyncDelegate = new MyAsyncDelegate(SendEmail);
                var asyncResult = asyncDelegate.BeginInvoke(obj, null, null);
            }
        }

        private void SendEmail(object obj)
        {
            MyTask myTask = (MyTask) obj;

            string textWhom = myTask.Parameter1;
            string textText = myTask.Parameter2;
  
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("serikrakhimov0807@gmail.com", "Серик Рахимов");
            // кому отправляем
            MailAddress to = new MailAddress(textWhom);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = textText;
            // текст письма
            m.Body = $"<h2>Письмо-тест работы smtp-клиента: {textText}</h2>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("serikrakhimov0807@gmail.com", "Magnum715331");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

    }
}
