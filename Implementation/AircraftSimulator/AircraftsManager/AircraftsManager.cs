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

            this.a_lateralMatrix = new ObservableCollection<float[]>();
            this.b_lateralMatrix = new ObservableCollection<float[]>();
            this.a_longitudinalMatrix = new ObservableCollection<float[]>();
            this.b_longitudinalMatrix = new ObservableCollection<float[]>();
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

        public void AddMissile(Missile.MissileType missileType, int sender, int missile_id, int targetId)
        {
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            instance.activeShooters[sender].AddMissile(missileType, missile_id, targetId);
        }

        public List<IData> GetShooterInitalData(int sender, IData additionalInformation = null)
        {
            //return new List<IData>();
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            List<IData> data = new List<IData>();
            data.Add(new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { sender } }, Sender = "client_id" });
            List <Common.Strategy> strategies = instance.activeShooters[sender].Context.Strategies;
            foreach (Common.Strategy strategy in strategies)
            {
                switch (strategy.ShooterType)
                {
                    case Shooters.ShooterType.F16:
                    case Shooters.ShooterType.F17:
                        data.AddRange((strategy as Aircraft.Strategy.AircraftStrategy).GetLateralInitialData(additionalInformation));
                        data.AddRange((strategy as Aircraft.Strategy.AircraftStrategy).GetLongitudinalInitialData(additionalInformation));
                        break;
                    default:
                        throw new Common.InvalidShooterTypeException();
                }
            }
            return data;
        }

        public List<IData> GetShooterInitalDataForTheSpecifiedStrategy(int sender, Shooters.ShooterType shooterType, IData additionalInformation = null)
        {
            //return new List<IData>();
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            List<IData> data = new List<IData>();
            data.Add(new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { sender } }, Sender = "client_id" });
            List<Common.Strategy> strategies = instance.activeShooters[sender].Context.Strategies;

            int strategy_id = -1;

            for(int i = 0; i < strategies.Count; i++)
            {
                if(strategies[i].ShooterType == shooterType)
                {
                    strategy_id = i;
                    break;
                }
            }

            data.Add(new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { strategy_id } }, Sender = "strategy_id" });
            Common.Strategy strategy = strategies[strategy_id];
                switch (strategy.ShooterType)
                {
                    case Shooters.ShooterType.F16:
                    case Shooters.ShooterType.F17:
                        data.AddRange((strategy as Aircraft.Strategy.AircraftStrategy).GetLateralInitialData(additionalInformation));
                        data.AddRange((strategy as Aircraft.Strategy.AircraftStrategy).GetLongitudinalInitialData(additionalInformation));
                        break;
                    default:
                        throw new Common.InvalidShooterTypeException();
                }
            return data;
        }

        public List<IData> GetShooterData(int sender, IData additionalInformation = null)
        {
            //return new List<IData>();
            if (!instance.activeShooters.ContainsKey(sender))
                throw new Shooter.InvalidShooterIdException();
            List<IData> data = new List<IData>();
            data.Add(new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { sender } }, Sender = "client_id" });
            List<Common.Strategy> strategies = instance.activeShooters[sender].Context.Strategies;
            foreach (Common.Strategy strategy in strategies)
            {
                switch (strategy.ShooterType)
                {
                    case Shooters.ShooterType.F16:
                    case Shooters.ShooterType.F17:
                        data.AddRange((strategy as Aircraft.Strategy.AircraftStrategy).GetLateralData(additionalInformation));
                        data.AddRange((strategy as Aircraft.Strategy.AircraftStrategy).GetLongitudinalData(additionalInformation));
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
            data.Add(new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { sender } }, Sender = "shooter_id" });
            data.Add(new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { missileId } }, Sender = "missile_id" });
            Missile.Missile missile = instance.activeShooters[sender].GetMissile(missileId);
            List <Common.Strategy> strategies = missile.MissileFlightContext.Strategies;
            foreach (Common.Strategy strategy in strategies)
            {
                switch ((strategy as Missile.Strategy.MissileStrategy).MissileType)
                {
                    case Missile.MissileType.M1:
                    case Missile.MissileType.M2:
                        data.Add((strategy as Missile.Strategy.MissileStrategy).GetTargetId());
                        break;
                    default:
                        throw new Common.InvalidShooterTypeException();
                }
            }
            return data;
        }

        public IData GetInitialAdditionalInformation(int sender)
        {
            Common.Strategy shooterStrategy = this.activeShooters[sender].Context.Strategies[0];
            switch(shooterStrategy.ShooterType)
            {
                case Shooters.ShooterType.F16:
                case Shooters.ShooterType.F17:
                    return (shooterStrategy as Aircraft.Strategy.AircraftStrategy).AdditionalInformation;
                default:
                    throw new Common.InvalidShooterTypeException();
            }

        }


        public Aircraft.Strategy.IAircraftParameters GetAircraftParameters()
        {
            List<Common.Strategy> shooterStrategies = this.activeShooters[activeShooter].Context.Strategies;
            foreach (Common.Strategy strategy in shooterStrategies)
            {
                if (strategy.ShooterType == ActiveShooterType)
                {
                    switch (ActiveShooterType)
                    {
                        case Shooters.ShooterType.F16:
                        case Shooters.ShooterType.F17:
                            return (strategy as Aircraft.Strategy.AircraftStrategy).AircraftParameters;
                    }
                }
            }
            return null;
        }

        public void RefreshAircraftParameters(Aircraft.Strategy.IAircraftParameters aircraftParameters)
        {
            List<Common.Strategy> shooterStrategies = this.activeShooters[activeShooter].Context.Strategies;
            foreach (Common.Strategy strategy in shooterStrategies)
            {
                if (strategy.ShooterType == ActiveShooterType)
                {
                    switch (ActiveShooterType)
                    {
                        case Shooters.ShooterType.F16:
                        case Shooters.ShooterType.F17:
                            (strategy as Aircraft.Strategy.AircraftStrategy).AircraftParameters = aircraftParameters;
                            (strategy as Aircraft.Strategy.AircraftStrategy).RefreshMatrixes();
                            SetActiveStrategy(ActiveShooterType);
                            break;
                    }
                }
            }
        }


        private ObservableCollection<Shooters.ShooterType> optionalStrategies;
        public ObservableCollection<Shooters.ShooterType> OptionalStrategies
        {
            get
            {
                return optionalStrategies;
            }
        }

        private int activeShooter = -1;

        public int ActiveShooter
        {
            get
            {
                return activeShooter;
            }
        }

        private Shooters.ShooterType activeShooterType;

        public Shooters.ShooterType ActiveShooterType
        {
            get
            {
                return activeShooterType;
            }
            set
            {
                activeShooterType = value;
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

        public void SetActiveShooter(int sender)
        {
            activeShooter = sender;
            SetActiveStrategies(sender);
        }

        private void SetActiveStrategies(int sender)
        {
            this.activeStrategies.Clear();
            this.optionalStrategies.Clear();
            foreach (Shooters.ShooterType shooterType in Enum.GetValues(typeof(Shooters.ShooterType)))
            {
                this.optionalStrategies.Add(shooterType);
            }
            
            if (instance.activeShooters.ContainsKey(sender))
            {
                foreach (Common.Strategy strategy in instance.activeShooters[sender].Context.Strategies)
                {
                    this.activeStrategies.Add(strategy.ShooterType);
                    this.optionalStrategies.Remove(strategy.ShooterType);
                }
            }
        }

        private ObservableCollection<float[]> a_lateralMatrix;

        public ObservableCollection<float[]> A_lateralMatrix
        {
            get
            {
                return a_lateralMatrix;
            }
        }


        private ObservableCollection<float[]> b_lateralMatrix;

        public ObservableCollection<float[]> B_lateralMatrix
        {
            get
            {
                return b_lateralMatrix;
            }
        }



        private ObservableCollection<float[]> a_longitudinalMatrix;

        public ObservableCollection<float[]> A_longitudinalMatrix
        {
            get
            {
                return a_longitudinalMatrix;
            }
        }


        private ObservableCollection<float[]> b_longitudinalMatrix;

        public ObservableCollection<float[]> B_longitudinalMatrix
        {
            get
            {
                return b_longitudinalMatrix;
            }
        }

        public void SetActiveStrategy(Shooters.ShooterType? activeShooterType)
        {
            foreach (Common.Strategy strategy in instance.activeShooters[activeShooter].Context.Strategies)
            {
                if(strategy.ShooterType == activeShooterType)
                {
                    if(activeShooterType == Shooters.ShooterType.F16 || activeShooterType == Shooters.ShooterType.F17)
                    {
                        SetActiveMatrixes((strategy as Aircraft.Strategy.AircraftStrategy).Matrixes);
                    }
                    return;
                }
            }
        }

        private void SetActiveMatrixes(List<IData> matrixes)
        {
            a_lateralMatrix.Clear();
            b_lateralMatrix.Clear();
            a_longitudinalMatrix.Clear();
            b_longitudinalMatrix.Clear();

            for (int i = 0; i < matrixes[0].Array.GetLength(0); i++)
            {
                a_lateralMatrix.Add(matrixes[0].Array[i]);
            }
            for (int i = 0; i < matrixes[1].Array.GetLength(0); i++)
            {
                b_lateralMatrix.Add(matrixes[1].Array[i]);
            }
            for (int i = 0; i < matrixes[2].Array.GetLength(0); i++)
            {
                a_longitudinalMatrix.Add(matrixes[2].Array[i]);
            }
            for (int i = 0; i < matrixes[3].Array.GetLength(0); i++)
            {
                b_longitudinalMatrix.Add(matrixes[3].Array[i]);
            }
        }

    }
}
