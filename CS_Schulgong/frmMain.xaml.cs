using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;
using System.ComponentModel;
//using System.Windows.Forms;
//using System.Drawing;

namespace CS_Schulgong
{
    /// <summary>
    /// InteraktionsLogik für MainWindow.xaml
    /// </summary>
    public partial class frmMain : Window, iForms
    {
        private readonly int StepSize = 20;
        private readonly int MaxSize = 140;
        private readonly string Format = "H:mm";
        private DispatcherTimer MyTimer;
        private cMainController MainController;

        public frmMain(cMainController newController)
        {
            InitializeComponent();
            this.MainController = newController;

            //Timer for updating the shown times
            MyTimer = new DispatcherTimer();
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Interval = new TimeSpan(0, 0, 1);
            MyTimer.Start();

            //Defaultvalue for the next possible Alarm is now + 1 Minute. //TODO: Prüfen ob bei Minute 1 bis 9 eine führende 0 angezeigt weird.: 
            txtAlarmTime.Text = DateTime.Now.Hour.ToString("00.##") + ":" + (DateTime.Now.Minute + 1).ToString("00.##");

            //Starting the programm no Alarm Exists, so the AlarmClocks should be invisible
            setAlarmClocksVisibility(false);

            //ADDED
            //++++++++++++++
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon(@"C:\Users\mrommel\Documents\VisualStudioProjekte\CS_Schulgong\CS_Schulgong\Resources\LKTools.ico"); 
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };

            //ADDED
            //++++++++++++++

            this.Show();
        }

        public void update(cControllers mainController)
        {
            List<DateTime> Alarms = MainController.getAlarms();

            lstAlarms.Items.Clear();
            foreach (DateTime Alarm in Alarms)
            {
                lstAlarms.Items.Add(Alarm.ToString(Format));
                
                if (lstAlarms.Height < this.MaxSize)
                {
                    lstAlarms.Height = lstAlarms.Height + StepSize;
                }
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MainController.AddAlarm(txtAlarmTime.Text) == false)
            {
                writeLog("Alarm entweder schon vorhanden oder in der Vergangenheit.", "red");
            }
        }
  
        private void lstAlarms_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DateTime MyAlarm;

            if (lstAlarms.SelectedItem != null) 
            {
                if (MainController.getAlarms().Count() > 0)
                {
                    try
                    {
                        if (DateTime.TryParseExact(lstAlarms.SelectedItem.ToString(), this.Format, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out MyAlarm))
                        {
                            lstAlarms.Items.Remove(lstAlarms.SelectedItem);
                            removeAlarm(MyAlarm);
                        }
                    }
                    catch (Exception ex)
                    {
                        writeLog(ex.ToString());
                    }
                }
            }
        }

        private void removeAlarm(DateTime MyAlarm)
        {
            MainController.RemoveAlarm(MyAlarm);

            //Change the height of the Alarm listbox
            if (lstAlarms.Height > StepSize)
            {
                lstAlarms.Height = lstAlarms.Height - StepSize;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            writeLog("Alarme pausiert",  "black");
            MainController.StopAlarms();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            writeLog("Alarme aktiv", "green");
            MainController.StartAlarms();
        }

        // Methode, die aufgerufen wird, wenn Timer-Event auftritt
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan TimeDifference;
            DateTime CurrentTime = DateTime.Now;
            string t = CurrentTime.ToString("H:mm:ss");
            lblCurrentTime.Content = t;

            if (MainController.getAlarms().Count() > 0)
            {
                DateTime NextAlarm = MainController.getAlarms()[0];
                    
                //Remove old Alarms
                if (NextAlarm < CurrentTime)
                {
                    removeAlarm(NextAlarm);
                }

                //Calculate remaining time till next Alarm
                if (NextAlarm > CurrentTime)
                {
                    lblNextAlarm.Content = NextAlarm.ToString("H:mm");

                    TimeDifference = NextAlarm - CurrentTime;
                    lblRemainingTime.Content = TimeDifference.ToString("hh\\:mm\\:ss");
                    setAlarmClocksVisibility(true);
                }
                else
                {
                    setAlarmClocksVisibility(false);
                }
            }
        }


        private void setAlarmClocksVisibility(bool IsVisible)
        {
            if (IsVisible == true)
            {
                lblNextAlarmHeader.Visibility = Visibility.Visible;
                lblRemaingTimeHeader.Visibility = Visibility.Visible;
                lblNextAlarm.Visibility = Visibility.Visible;
                lblRemainingTime.Visibility = Visibility.Visible;
            }
            else
            {
                lblNextAlarmHeader.Visibility = Visibility.Hidden;
                lblRemaingTimeHeader.Visibility = Visibility.Hidden;
                lblNextAlarm.Visibility = Visibility.Hidden;
                lblRemainingTime.Visibility = Visibility.Hidden;
            }
        }


        private void writeLog(string MyText, string Color = "black")
        {
            switch (Color)
            {
                case "green":
                    lblInformation.Foreground = Brushes.Green;
                    break;
                case "red":
                    lblInformation.Foreground = Brushes.Red;
                    break;
                default:
                    lblInformation.Foreground = Brushes.Black;
                    break;
            }
            lblInformation.Text = MyText;
        }

        private void btnDefaultValues_Click(object sender, RoutedEventArgs e)
        {
            MainController.addTestAlarms();
        }

        private void txtAlarmTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                MainController.AddAlarm(txtAlarmTime.Text);
            }
        }

        private void btnGong_Click(object sender, RoutedEventArgs e)
        {
            MainController.playGong();         
        }

        private void btnSilent_Click(object sender, RoutedEventArgs e)
        {
            MainController.stopGong();
        }

   
        private void chkTopMost_Click(object sender, RoutedEventArgs e)
        {
            if (chkTopMost.IsChecked == true)
            {
                this.Topmost = true;     
            }
            else
            {
                this.Topmost = false;
            }
        }



        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized && chkMinimizToSystemTray.IsChecked == true)
                this.Hide();

            base.OnStateChanged(e);
        }


    }
}
