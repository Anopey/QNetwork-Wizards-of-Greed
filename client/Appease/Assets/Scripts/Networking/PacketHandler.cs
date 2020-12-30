using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Networking
{
    [Serializable]
    public abstract class PacketHandler : ScriptableObject
    {

        private TypeCode[] expectedPrimitives;

        public TypeCode[] ExpectedPrimitives { get { return expectedPrimitives; } }

        public void OnPacketRecieved(Packet packet)
        {
            ProcessPacket(packet);
        }

        protected abstract void ProcessPacket(Packet packet);

    }

}
