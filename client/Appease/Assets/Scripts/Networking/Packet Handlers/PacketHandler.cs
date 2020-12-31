using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Networking
{
    [Serializable]
    public abstract class PacketHandler : ScriptableObject
    {

        public void OnPacketRecieved(Packet packet)
        {
            ProcessPacket(packet);
        }

        protected abstract void ProcessPacket(Packet packet);

    }

}
