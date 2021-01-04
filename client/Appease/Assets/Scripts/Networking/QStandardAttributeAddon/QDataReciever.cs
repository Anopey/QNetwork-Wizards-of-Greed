using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.QNetwork.StandardAttributeAddon
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class QDataReciever : Attribute
    {
        public ushort PacketID { get; private set; }
        public string Description { get; private set; }

        public QDataReciever(ushort packetID, string desc = "")
        {
            PacketID = packetID;
            Description = desc;
        }
    }

}

