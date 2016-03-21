using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AircraftsManager
{
    sealed public partial class AircraftsManager : Initializer
    {
        private static AircraftsManager instance;
        private Dictionary<int, Shooter.Shooter> activeShooters;
        private Shooter.ShooterFactory.ShooterCreator shooterCreator;

        public static AircraftsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AircraftsManager();
                return instance;
            }
        }

        private AircraftsManager()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            activeShooters = new Dictionary<int, Shooter.Shooter>();
        }

        public void AddShooter(Shooter.ShooterType shooterType, int sender)
        {
            PrepareShooterCreator(shooterType);
            this.activeShooters.Add(sender, shooterCreator.ShooterFactoryMethod(shooterType));
        }

        public void AddStrategy(Shooter.ShooterType shooterType, int sender)
        {
            this.activeShooters[sender].context.AddStrategy(Common.Strategy.GetSpecificStrategy(shooterType));
        }
    }
}
