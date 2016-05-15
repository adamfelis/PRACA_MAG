using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Main
{
    public partial class App : System.Windows.Application
    {

        void Start(object sender, StartupEventArgs args)
        {
            Main.MainWindow mainWindow = new Main.MainWindow();
            mainWindow.Show();
            //ApplicationStartup app = new ApplicationStartup();
            //app.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
            //app.Run();
        }
    }
}
