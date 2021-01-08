using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QNetwork
{
    public class PacketManager
    {

        private Dictionary<int, PacketDataType> idToPacketDataType;

        public PacketManager(PacketManagerArgs packetManagerArgs)
        {
            idToPacketDataType = new Dictionary<int, PacketDataType>();

            foreach(var data in packetManagerArgs.handledTypes)
            {
                if (idToPacketDataType.ContainsKey(data.ID))
                {
                    Debug.LogError("Two packet types of same ID were tried to be handled in the same scene!");
                }
                idToPacketDataType.Add(data.ID, data);
            }

            foreach(var handler in idToPacketDataType.Values)
            {
                handler.InitializeDefaultHandlers();
            }
        }

        public PacketDataType GetPacketDataFromID(ushort ID)
        {
            return idToPacketDataType[ID];
        }

        public bool IDIsValid(ushort ID)
        {
            return idToPacketDataType.ContainsKey(ID);
        }
    }

    [Serializable]
    public struct PacketManagerArgs
    {
        public PacketDataType[] handledTypes;
    }
}

