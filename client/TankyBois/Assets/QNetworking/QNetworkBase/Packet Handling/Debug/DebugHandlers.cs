﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QNetwork.Debugging
{
    using QNetwork;
    using System;

    [CreateAssetMenu(fileName = "(any) Debug", menuName = "QNetwork/DefaultPacketHandlers/(any) Debug", order = 1)]
    public class DebugHandlers : PacketHandler, IMultiplePacketTypeHandler
    {
        private class DebugHandler
        {
            private TypeCode[] expected;

            public DebugHandler(TypeCode[] expected)
            {
                this.expected = expected;
            }
            public void ProcessPacket(Packet packet)
            {
                string msg = "Server sent: ";
                for (int i = 0; i < expected.Length; i++)
                {
                    msg += packet.ReadAsString(expected[i]);
                }
                Debug.Log(msg);
            }
        }

        public Action<Packet> VerifyInitializePacket(TypeCode[] inputTypes)
        {
            DebugHandler inst = new DebugHandler(inputTypes);
            return inst.ProcessPacket;
        }

        protected override void OnGameStart()
        {
           
        }
    }

}

