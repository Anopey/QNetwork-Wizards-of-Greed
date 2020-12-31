using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Debugging
{
    using Networking;
    using System;

    [CreateAssetMenu(fileName = "DebugFirstString", menuName = "Networking/PacketHandlers/DebugFirstString", order = 1)]
    public class DebugHandlers : PacketHandler, IDynamicPacketHandler
    {
        private TypeCode[] expected;


        public bool VerifyInitializePacket(TypeCode[] inputTypes)
        {
            expected = inputTypes;
            return true;
        }

        protected override void ProcessPacket(Packet packet)
        {
            string msg = "Server sent: ";
            
            Debug.Log("Server sent: " + packet.ReadString());
        }
    }

}

