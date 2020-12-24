using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Networking
{
    public class PacketManager
    {

        public PacketManager(PacketManagerArgs packetManagerArgs)
        {

        }

    }

    [Serializable]
    public struct PacketManagerArgs
    {
        public PacketDataType[] handledTypes;
    }
}

