using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ApplicationManager
{
    public sealed class MainApplication : Initializer
    {
        private static MainApplication instance;
        private AircraftsManagerCommunication.IAircraftsManagerCommunication aircraftsManagerCommunication;
        private ToolsManagerCommunication.IToolsManagerCommunication toolsManagerCommunication;
        private ServerCommunication.IServerCommunication serverCommunication;

        public static MainApplication Instance
        {
            get
            {
                if (instance == null)
                    instance = new MainApplication();
                return instance;
            }
        }

        internal ToolsManagerCommunication.IToolsManagerCommunication ToolsManagerCommunication
        {
            get
            {
                return toolsManagerCommunication;
            }
        }

        protected override void Initialize()
        {
            this.aircraftsManagerCommunication = new AircraftsManagerCommunication.AircraftsManagerCommunication();
            this.toolsManagerCommunication = new ToolsManagerCommunication.ToolsManagerCommunication();
            this.serverCommunication = new ServerCommunication.ServerCommunication();
        }

        private MainApplication()
        {
            Initialize();
        }
    }
}
