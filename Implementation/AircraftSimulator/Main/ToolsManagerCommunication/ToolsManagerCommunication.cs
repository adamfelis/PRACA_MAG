namespace Main.ToolsManagerCommunication
{
    class ToolsManagerCommunication : IToolsManagerCommunication
    {
        public ToolsManager.ToolsManager ManagerInstance => ToolsManager.ToolsManager.Instance;
    }
}
