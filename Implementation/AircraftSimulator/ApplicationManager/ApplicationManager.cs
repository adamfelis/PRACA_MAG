using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ApplicationManager
{
    public sealed partial class ApplicationManager : Initializer
    {
        private static ApplicationManager instance;
        private AircraftsManagerCommunication.IAircraftsManagerCommunication aircraftsManagerCommunication;
        private ToolsManagerCommunication.IToolsManagerCommunication toolsManagerCommunication;
        private ServerCommunication.IServerCommunication serverCommunication;

        public static ApplicationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationManager();
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

        private ApplicationManager()
        {
            Initialize();
        }
    }
}
