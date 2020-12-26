using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Networking
{
    [CreateAssetMenu(fileName = "NetworkManager", menuName = "Networking/PacketHandler", order = 2)]
    public class PacketHandler : ScriptableObject
    {

        [SerializeField]
        private TypeCode[] expectedPrimitives;

        public TypeCode[] ExpectedPrimitives { get { return expectedPrimitives; } }

        public void ProcessPacket(Packet packet)
        {

        }

    }

}
