using Common.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager.Missile.Strategy
{
    abstract class MissileStrategy : Common.Strategy
    {
        private int target_id;
        internal int TargetID
        {
            set
            {
                target_id = value;
            }
        }

        private MissileType missileType;

        internal MissileType MissileType
        {
            get
            {
                return missileType;
            }
            set
            {
                missileType = value;
            }
        }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }

        public IData GetTargetId()
        {
            return new Data() { InputType = DataType.Float, Array = new float[1][] { new float[1] { target_id } }, Sender = "target_id" };
        }
    }
}
