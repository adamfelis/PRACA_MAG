using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Containers;
using System.Collections.ObjectModel;

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
            this.activeStrategies = new ObservableCollection<Shooters.ShooterType>();
            this.optionalStrategies = new ObservableCollection<Shooters.ShooterType>();
        }

        public void AddShooter(Shooters.ShooterType shooterType, int sender)
        {
            if (instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            PrepareShooterCreator(shooterType);
            instance.activeShooters.Add(sender, shooterCreator.ShooterFactoryMethod(shooterType));
        }

        public void AddStrategy(Shooters.ShooterType shooterType, int sender)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            instance.activeShooters[sender].Context.AddStrategy(Common.Strategy.GetSpecificShooterStrategy(shooterType));
            SetActiveStrategies(sender);
        }

        public void RemoveStrategy(Shooters.ShooterType shooterType, int sender)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            instance.activeShooters[sender].Context.RemoveStrategy(shooterType);
            SetActiveStrategies(sender);
        }

        public void AddMissile(Missile.MissileType missileType, int sender)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            instance.activeShooters[sender].AddMissile(missileType);
        }
        
        public List<IData> GetShooterData(int sender, IData additionalInformation = null)
        {
            //return new List<IData>();
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            List<IData> data = new List<IData>();
            List<Common.Strategy> strategies = instance.activeShooters[sender].Context.Strategies;
            foreach (Common.Strategy strategy in strategies)
            {
                switch (strategy.ShooterType)
                {
                    case Shooters.ShooterType.F16:
                    case Shooters.ShooterType.F17:
                        data.Add((strategy as Aircraft.Strategy.AircraftStrategy).GetLateralData(additionalInformation));
                        data.Add((strategy as Aircraft.Strategy.AircraftStrategy).GetLongitudinalData(additionalInformation));
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


        private ObservableCollection<Shooters.ShooterType> optionalStrategies;
        public ObservableCollection<Shooters.ShooterType> OptionalStrategies
        {
            get
            {
                return optionalStrategies;
            }
        }

        private ObservableCollection<Shooters.ShooterType> activeStrategies;
        public ObservableCollection<Shooters.ShooterType> ActiveStrategies
        {
            get
            {
                return activeStrategies;
            }
        }

        public void SetActiveStrategies(int sender)
        {
            //ObservableCollection<Shooter.ShooterType> result = new ObservableCollection<Shooter.ShooterType>();
            this.activeStrategies.Clear();
            this.optionalStrategies.Clear();
            foreach (Shooters.ShooterType shooterType in Enum.GetValues(typeof(Shooters.ShooterType)))
            {
                this.optionalStrategies.Add(shooterType);
            }
            
            if (instance.activeShooters.ContainsKey(sender))
            {
                //this.activeStrategies = instance.activeShooters[sender].Context.TypesOfStrategies;
                foreach (Common.Strategy strategy in instance.activeShooters[sender].Context.Strategies)
                {
                    this.activeStrategies.Add(strategy.ShooterType);
                    this.optionalStrategies.Remove(strategy.ShooterType);
                }
            }
        }
    }
}
