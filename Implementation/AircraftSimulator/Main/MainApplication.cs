using Common;
using Main.AircraftsManagerCommunication;
using Main.ServerCommunication;
using Main.ToolsManagerCommunication;

namespace Main
{
    public sealed class MainApplication : Initializer
    {
        private static MainApplication instance;
        private IAircraftsManagerCommunication aircraftsManagerCommunication;
        private IToolsManagerCommunication toolsManagerCommunication;
        private IServerCommunication serverCommunication;

        public static MainApplication Instance
        {
            get
            {
                if (instance == null)
                    instance = new MainApplication();
                return instance;
            }
        }

        internal IAircraftsManagerCommunication AircraftsManagerCommunication
        {
            get
            {
                return aircraftsManagerCommunication;
            }
        }

        internal IToolsManagerCommunication ToolsManagerCommunication
        {
            get
            {
                return toolsManagerCommunication;
            }
        }

        internal IServerCommunication ServerCommunication
        {
            get
            {
                return serverCommunication;
            }
        }

        protected override void Initialize()
        {
            aircraftsManagerCommunication = new AircraftsManagerCommunication.AircraftsManagerCommunication();
            toolsManagerCommunication = new ToolsManagerCommunication.ToolsManagerCommunication();
            serverCommunication = new ServerCommunication.ServerCommunication();
        }

        public MainApplication()
        {
            Initialize();
        }
    }
}
