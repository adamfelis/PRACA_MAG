using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    public class MathHelper
    {
        private static MathHelper instance;

        public static MathHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new MathHelper();
                return instance;
            }
        }

        private MathHelper() { }
    }
}
