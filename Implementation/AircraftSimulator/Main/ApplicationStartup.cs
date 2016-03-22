using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class ApplicationStartup : System.Windows.Application
    {
        [STAThread]

        public static void Main()

        {

            ApplicationStartup app = new ApplicationStartup();

            app.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);

            app.Run();

        }
    }
}
