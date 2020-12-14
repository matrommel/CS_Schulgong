using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CS_Schulgong
{
   
    /// <summary>
    /// InteraktionsLogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {

        cMainController MainController;
        new frmMain MainWindow;

        public App()
        {
            this.MainController = new cMainController();
            this.MainWindow = new frmMain(this.MainController);
            this.MainController.registriereForm(this.MainWindow);
        }



        
    }
}


