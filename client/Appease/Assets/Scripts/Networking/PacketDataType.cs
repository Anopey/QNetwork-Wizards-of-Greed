using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Game.Networking
{
    [CreateAssetMenu(fileName = "PacketDataType", menuName = "Networking/PacketDataType", order = 1)]
    public class PacketDataType : ScriptableObject
    {

        public string Description;

        public ushort ID;

        public UnityEvent<Packet> Handler;

        public TypeCode[] Primitives;

    }

}

