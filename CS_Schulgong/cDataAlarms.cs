using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Schulgong
{
    public class cDataAlarms : iDataModell
    {
        private List<DateTime> Alarms = new List<DateTime>();

        public List<DateTime> getValues()
        {
            Alarms.Sort();
            return Alarms;
        }

        public bool removeValue(DateTime Alarm)
        {
            bool IsRemoved = false;

            if (Alarms != null)
            {   
                Alarms.Remove(Alarm);
                IsRemoved = true;
            }
            
            return IsRemoved;
        }

        public bool setValues(DateTime Alarm)
        {
            bool IsSet = false;

            if (!Alarms.Contains(Alarm))
            {
                Alarms.Add(Alarm);
                IsSet = true;
            }
           
            return IsSet;
        }

  

        public bool Exists(DateTime myValue)
        {
            bool Exists = false;

            if (Alarms.Contains(myValue))
            {
                Exists = true;
            }

            return Exists;
        }


    }
}
