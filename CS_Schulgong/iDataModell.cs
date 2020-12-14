using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Schulgong
{
    public interface iDataModell
    {

        bool setValues(DateTime Alarm);

        List<DateTime> getValues();

        bool removeValue(DateTime Alarm);

        bool Exists(DateTime myValue);

    }
}
