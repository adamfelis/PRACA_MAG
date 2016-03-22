namespace Main.AircraftsManagerCommunication
{
    class AircraftsManagerCommunication : IAircraftsManagerCommunication
    {
        public AircraftsManager.AircraftsManager ManagerInstance
        {
            get
            {
                return AircraftsManager.AircraftsManager.Instance;
            }
        }
    }
}
