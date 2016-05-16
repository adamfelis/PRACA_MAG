using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public interface ICloudCreator
    {
        void NotifyCenterChanged(Transform transform);
    }
}
