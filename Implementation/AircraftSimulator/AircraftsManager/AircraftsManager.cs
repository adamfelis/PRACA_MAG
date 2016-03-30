using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;

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
                {
                    instance = new AircraftsManager();
                }
                return instance;
            }
        }

        private AircraftsManager()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            this.activeShooters = new Dictionary<int, Shooter.Shooter>();
        }

        public void AddShooter(Shooter.ShooterType shooterType, int sender)
        {
            if (instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            PrepareShooterCreator(shooterType);
            instance.activeShooters.Add(sender, shooterCreator.ShooterFactoryMethod(shooterType));
        }

        public void AddStrategy(Shooter.ShooterType shooterType, int sender)
        {
            if(!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            instance.activeShooters[sender].Context.AddStrategy(Common.Strategy.GetSpecificShooterStrategy(shooterType));
        }

        public void AddMissile(Missile.MissileType missileType, int sender)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            instance.activeShooters[sender].AddMissile(missileType);
        }
        
        public List<IData> GetShooterData(int sender)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            List<IData> data = new List<IData>();
            List<Common.Strategy> strategies = instance.activeShooters[sender].Context.Strategies;
            foreach (Common.Strategy strategy in strategies)
            {
                switch (strategy.ShooterType)
                {
                    case Shooter.ShooterType.F16:
                        data.Add((strategy as Aircraft.Strategy.ConcreteStrategies.ConcreteAircraftStrategyF16).GetLateralData());
                        data.Add((strategy as Aircraft.Strategy.ConcreteStrategies.ConcreteAircraftStrategyF16).GetLongitudinalData());
                        break;
                    case Shooter.ShooterType.F17:
                        data.Add((strategy as Aircraft.Strategy.ConcreteStrategies.ConcreteAircraftStrategyF17).GetLateralData());
                        data.Add((strategy as Aircraft.Strategy.ConcreteStrategies.ConcreteAircraftStrategyF17).GetLongitudinalData());
                        break;
                    default:
                        throw new Common.InvalidShooterTypeException();
                }
            }
            return data;
        }

        // TODO : return type has to be changed
        public List<IData> GetMissileData(int sender, int missileId)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            List<IData> data = new List<IData>();
            Missile.Missile missile = instance.activeShooters[sender].GetMissile(missileId);
            List <Common.Strategy> strategies = missile.MissileFlightContext.Strategies;
            foreach (Common.Strategy strategy in strategies)
            {
                switch ((strategy as Missile.Strategy.MissileStrategy).MissileType)
                {
                    case Missile.MissileType.M1:
                        //(strategy as Missile.Strategy.ConcreteStrategies.ConcreteMissileStrategyM1).;
                        break;
                    case Missile.MissileType.M2:
                        //(strategy as Missile.Strategy.ConcreteStrategies.ConcreteMissileStrategyM1).;
                        break;
                    default:
                        throw new Common.InvalidShooterTypeException();
                }
            }
            return data;
        }
    }
}
