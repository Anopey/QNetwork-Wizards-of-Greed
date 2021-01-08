using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace QNetwork
{
    interface ISinglePacketTypeHandler
    {

        ReadOnlyCollection<TypeCode> ExpectedPrimitives { get; }

        void ProcessPacket(Packet p);

    }
}
