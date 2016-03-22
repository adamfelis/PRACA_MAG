namespace Main.ServerCommunication
{
    interface IServerCommunication
    {
        Server.IServer ServerInstance
        {
            get;
        }
    }
}
