﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AircraftData
{
    public class MissileData
    {
        private float[][] missileData;

        //velocities
        #region
        public float U_0
        {
            get { return missileData[0][0]; }
        }
        public float V_0
        {
            get
            {
                return missileData[0][1];
            }
        }
        public float W_0
        {
            get { return missileData[0][2]; }
        }
        #endregion

        //positions
        #region
        public float X_0
        {
            get { return missileData[1][0]; }
        }
        public float Y_0
        {
            get
            {
                return missileData[1][1];
            }
        }
        public float Z_0
        {
            get { return missileData[1][2]; }
        }
        #endregion

        public MissileData(float[][] missileData)
        {
            this.missileData = missileData;
        }

        public MissileData(float U_0, float V_0, float W_0, float X_0, float Y_0, float Z_0)
        {
            this.missileData = new float[][]
            {
                new float[] { U_0, V_0, W_0 },
                new float[] { X_0, Y_0, Z_0 }
            };
        }

        public float[][] GetData()
        {
            return this.missileData;
        }
    }
}