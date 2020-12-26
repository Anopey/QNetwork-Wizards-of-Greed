using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Debugging
{
    using Networking;

    public class DebugHandlers : ScriptableObject
    {

        public static void HandleSimpleMessagePacket(Packet p)
        {
            Debug.Log(p.ReadString());
        }



    }

}

