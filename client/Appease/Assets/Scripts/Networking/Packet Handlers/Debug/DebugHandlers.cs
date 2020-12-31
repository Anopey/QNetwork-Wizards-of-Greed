using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Debugging
{
    using Networking;
    using System;

    [CreateAssetMenu(fileName = "DebugFirstString", menuName = "Networking/PacketHandlers/DebugFirstString", order = 1)]
    public class DebugHandlers : PacketHandler
    {
        private TypeCode[] expected = new TypeCode[] { TypeCode.String};

        public override TypeCode[] ExpectedPrimitives { get { return expected; } }

        protected override void ProcessPacket(Packet packet)
        {
            Debug.Log("Server sent: " + packet.ReadString());
        }
    }

}

