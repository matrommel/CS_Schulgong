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


namespace CS_Schulgong
{
    public class cMainController : cControllers
    {
        private List<cAlarmClock> Clocks = new List<cAlarmClock>();
        private string Format = "H:mm";
        private string Log;
        private cDataAlarms Alarms; 
        
        public cMainController()
        {
            Alarms = new cDataAlarms();
        }


        public bool AddAlarm(string AlarmTime)
        {
            DateTime MyAlarm;
            bool isAdded = false;

            //check if string is DateTime
            if (DateTime.TryParseExact(AlarmTime, this.Format, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out MyAlarm))
            {

                //Only add future Alarms 
                if (MyAlarm > DateTime.Now)
                {
                   //Only add Alarms, that doesn't exist yet
                    if (Alarms.setValues(MyAlarm))
                    {
                        Clocks.Add(new cAlarmClock(MyAlarm));
                        StartAlarms();
                        notify();
                        isAdded = true;
                    }
                    else
                    {
                        writeLog("Eintrag schon vorhanden");
                    }
                }
            }
            else
            {
                writeLog("Uhrzeit: " + AlarmTime + " nicht korrekt angegeben");
            }

            return isAdded;
        }


        public void StartAlarms()
        {
            foreach (cAlarmClock Clock in Clocks)
            {
               //Only future Alarms are started
                if (Clock.getAlarmTime() > DateTime.Now)
                {
                    Clock.MyTimer_start();
                }
         
            }

            writeLog("Alarme aktiv");
        }

        public void StopAlarms()
        {
            foreach (cAlarmClock Clock in Clocks)
            {
                Clock.MyTimer_Stop();
            }

            writeLog("Alarme inaktiv");
        }

        public bool RemoveAlarm(DateTime AlarmTime)
        {
            cAlarmClock RemoveClock;

            if (Alarms != null)
            {
                //Entferne Alarm
                Alarms.removeValue(AlarmTime);
                

                //Find the correct Clock
                foreach (cAlarmClock Clock in Clocks)
                {
                    //Hole die Referenz für den übertragenen Alarm
                    RemoveClock = Clock.getClockByAlarm(AlarmTime);
                    //Falls tatslächlich etwas gefunden wurde, dann entferne den Verweis.
                    if (RemoveClock != null)
                    {
                        Clocks.Remove(RemoveClock);
                        notify();
                        return true;
                    }
                }
            }
            
            return false;
        }


        public List<DateTime> getAlarms()
        {
            return Alarms.getValues();
        }

        private void writeLog(string MyText)
        {
            Log = MyText;
        }

        public string leseLog()
        {
            return Log;
        }

        public void playGong()
        {           
            cAlarmClock Clock = new cAlarmClock();
            Clocks.Add(Clock);

            Clock.playSound();
        }

        public void stopGong()
        {
            if (Clocks.Count() > 0)
            {
                foreach (cAlarmClock Clock in Clocks)
                {
                    Clock.stopSound();
                }
            }
        }


        public void addTestAlarms()
        {

            TimeSpan now = DateTime.Now.TimeOfDay;
            List<TimeSpan> AlarmTimes = new List<TimeSpan>();

            AlarmTimes.Add(new TimeSpan(7, 40, 0));
            AlarmTimes.Add(new TimeSpan(7, 45, 0));
            AlarmTimes.Add(new TimeSpan(8, 30, 0));
            AlarmTimes.Add(new TimeSpan(9, 15, 0));
            AlarmTimes.Add(new TimeSpan(9, 30, 0));
            AlarmTimes.Add(new TimeSpan(10, 15, 0));
            AlarmTimes.Add(new TimeSpan(11, 00, 0));
            AlarmTimes.Add(new TimeSpan(11, 15, 0));
            AlarmTimes.Add(new TimeSpan(12, 00, 0));
            AlarmTimes.Add(new TimeSpan(12, 45, 0));
            AlarmTimes.Add(new TimeSpan(13, 30, 0));
            AlarmTimes.Add(new TimeSpan(14, 15, 0));
            AlarmTimes.Add(new TimeSpan(15, 0, 0));
            AlarmTimes.Add(new TimeSpan(15, 15, 0));
            AlarmTimes.Add(new TimeSpan(15, 45, 0));
            AlarmTimes.Add(new TimeSpan(16, 0, 0));

            foreach (var item in AlarmTimes)
            {
                if (now < item)
                {
                    AddAlarm(item.ToString("hh\\:mm"));
                }
                else
                {
                    writeLog("Zeiten liegen in der Vergangenheit und sind nicht mehr erreichbar!");
                }
            }
        }

     }
}


