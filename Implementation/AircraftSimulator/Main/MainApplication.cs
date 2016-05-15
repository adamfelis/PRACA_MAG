using Common;
using Main.AircraftsManagerCommunication;
using Main.ServerCommunication;
using Main.ToolsManagerCommunication;
using System;

namespace Main
{
    public sealed class MainApplication : Initializer, IAircraftsManagerCommunicationImplementor, IToolsManagerCommunicationImplementor
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

        public IAircraftsManagerCommunication AircraftsManagerCommunication
        {
            get
            {
                return aircraftsManagerCommunication;
            }
        }

        public IToolsManagerCommunication ToolsManagerCommunication
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
            toolsManagerCommunication = new ToolsManagerCommunication.ToolsManagerCommunication();
            aircraftsManagerCommunication = new AircraftsManagerCommunication.AircraftsManagerCommunication();
        }

        public void SetActionForToolsAdding(Action<ToolAdapter.Tool.ITool> toolAddedAction)
        {
            toolsManagerCommunication.ManagerInstance.SetActionForToolsAdding(toolAddedAction);
        }

        public void InformAboutDispatcher(Common.IDispatchable iDispatchable)
        {
            serverCommunication = new ServerCommunication.ServerCommunication(iDispatchable, this as IAircraftsManagerCommunicationImplementor,
                this as IToolsManagerCommunicationImplementor);
            serverCommunication.ServerInstance.StartServer();
        }

        public void InformAboutScriptTypeForComputations(global::Common.Scripts.ScriptType scriptType)
        {
            ToolsManagerCommunication.ManagerInstance.SetScriptTypeForComputations(scriptType);
        }

        public MainApplication()
        {
            Initialize();
        }
    }
}
