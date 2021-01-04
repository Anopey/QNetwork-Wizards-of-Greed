using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.QNetwork
{
    interface ISinglePacketTypeHandler
    {

        TypeCode[] ExpectedPrimitives { get; }

        void ProcessPacket(Packet p);

    }
}
