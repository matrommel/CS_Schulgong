using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace CS_Schulgong
{
    public class cAlarmClock
    {
        private EventHandler AlarmEvent;
        private Timer MyTimer;
        private DateTime AlarmTime;
        private bool IsEnabled;
        private SoundPlayer MyPlayer;

        public cAlarmClock(DateTime Alarm)
        {
            this.AlarmTime = Alarm;
            MyTimer = new Timer();
            MyTimer_start();

            IsEnabled = true;
        }

        public cAlarmClock()
        {

        }

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {    
            if (IsEnabled == true && DateTime.Now.Hour == AlarmTime.Hour && DateTime.Now.Minute == AlarmTime.Minute)
            {
                IsEnabled = false;
                OnAlarm();
                MyTimer.Stop();
                playSound();
            }
        }

        public void MyTimer_start()
        {
            MyTimer.Elapsed += MyTimer_Elapsed;
            MyTimer.Interval = 1000;
            MyTimer.Start();
        }

        public void MyTimer_Stop()
        {
            if (MyTimer != null)
            {
                MyTimer.Stop();
                IsEnabled = false;
            }

        }

        protected virtual void OnAlarm()
        {
            if (AlarmEvent != null)
            {
                AlarmEvent(this, EventArgs.Empty);

            }
           
        }


        public event EventHandler Alarm
        {
            add { AlarmEvent += value; }
            remove { AlarmEvent -= value; }
        }



        public void stopSound()
        {
            if (MyPlayer != null)
            {
                MyPlayer.Stop();
            }
        }

        public cAlarmClock getClockByAlarm(DateTime Alarm)
        {
            if (Alarm == this.AlarmTime)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public DateTime getAlarmTime()
        {
            return AlarmTime;
        }


        public void playSound()
        {
            try
            {
                MyPlayer = new SoundPlayer();
                MyPlayer.SoundLocation = @"C:\Users\mrommel\Documents\VisualStudioProjekte\CS_Schulgong\CS_Schulgong\audio\Gong.wav";
                MyPlayer.Play();
                //TODO: Entferne Item aus der Liste
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Messege", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
