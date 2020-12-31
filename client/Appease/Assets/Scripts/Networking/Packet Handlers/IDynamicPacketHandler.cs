using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Networking
{
    interface IDynamicPacketHandler
    {
        /// <summary>
        /// Returns the handler if can handle this input and has been initialized. Returns false otherwise.
        /// </summary>
        /// <param name="inputTypes"></param>
        /// <returns></returns>
        Action<Packet> VerifyInitializePacket(TypeCode[] inputTypes);

    }
}
