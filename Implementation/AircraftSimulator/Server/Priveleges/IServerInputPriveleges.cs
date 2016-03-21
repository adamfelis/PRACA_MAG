using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IServerInputPriveleges
    {
        void RespondToClient(int id, float[,] arr);
    }
}
