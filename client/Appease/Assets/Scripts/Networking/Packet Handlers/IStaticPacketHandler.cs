using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Networking
{
    interface IStaticPacketHandler
    {

        TypeCode[] ExpectedPrimitives { get; }

        void ProcessPacket(Packet p);

    }
}
