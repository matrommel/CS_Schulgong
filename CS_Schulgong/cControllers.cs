using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Schulgong
{
    public class cControllers
    {

        private List<iForms> MainWindows = new List<iForms>();

        public void registriereForm(iForms NewWindow)
        {
            this.MainWindows.Add(NewWindow);
        }

        public bool entferneForm(iForms OldWindow)
        {
            if (OldWindow != null)
            {
                this.MainWindows.Remove(OldWindow);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void notify()
        {
            foreach (iForms item in this.MainWindows)
            {
                item.update(this);
            }
        }

        public List<iForms> getMainWindows()
        {
            return MainWindows;
        }

    }
}
