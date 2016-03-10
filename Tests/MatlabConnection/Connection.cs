using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLApp;

namespace MatlabConnection
{
    public class Connection
    {
        public static Connection Instance
        {
            get
            {
                if (instance == null)
                    instance = new Connection();
                return instance;
            }
        }

        private static MLApp.MLApp mlApp;
        private static Connection instance;

        private Connection()
        {
            mlApp = new MLApp.MLApp();
        }

        public object[] ExecuteCommand()
        {
            //mlApp.Execute(@"cd E:\PW\PRACA_MAG\Tests\TestProjectWithMatlab\Assets");
            object result = null;
            mlApp.Feval("CalcInverse", 1, out result);
            object[] res = result as object[];
            return res;
        }
    }
}
