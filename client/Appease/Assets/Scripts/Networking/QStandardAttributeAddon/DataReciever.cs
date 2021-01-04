using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Networking
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class DataReciever : Attribute
    {
        public ushort PacketID { get; private set; }
        public string Description { get; private set; }

        public DataReciever(ushort packetID, string desc = "")
        {
            PacketID = packetID;
            Description = desc;
        }
    }

}

