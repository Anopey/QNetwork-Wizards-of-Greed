﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.ObjectModel;

namespace QNetwork.Infrastructure
{
    using QNetwork;
    using System;


    [CreateAssetMenu(fileName = "(0) AcknowledgementHandler", menuName = "QNetwork/DefaultPacketHandlers/(0) AcknowledgementHandler", order = 2)]
    public class AcknowledgementHandler : PacketHandler, ISinglePacketTypeHandler
    {

        [SerializeField]
        private bool debugErrorUponUnhandled = false;

        public static AcknowledgementHandler Singleton { get; private set; }

        private Dictionary<ushort, List<AcknowledgementHandle>> idToRegisteredHandles = new Dictionary<ushort, List<AcknowledgementHandle>>();

        private TypeCode[] expectedPrimitives = new TypeCode[]
        {
            TypeCode.String,
            TypeCode.UInt16
        };

        public ReadOnlyCollection<TypeCode> ExpectedPrimitives { get { return new ReadOnlyCollection<TypeCode>(expectedPrimitives); } }

        public void ProcessPacket(Packet p)
        {
            string err = p.ReadString();
            ushort id = p.ReadUShort();

            if (!idToRegisteredHandles.ContainsKey(id))
            {
                if (debugErrorUponUnhandled)
                {
                    Debug.LogError("The acknowledgement processor was asked to process a packet with ID " + id.ToString() + " and yet no such handles were attached to it!");
                }
                return;
            }

            if (err == "")
            {
                //no error. have a good day.
                var handlers = idToRegisteredHandles[id];
                for (int i = 0; i < handlers.Count; i++)
                {
                    handlers[i].onSuccess();
                    if (handlers[i].callOnce)
                    {
                        handlers.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                // D:
                var handlers = idToRegisteredHandles[id];
                for (int i = 0; i < handlers.Count; i++)
                {
                    handlers[i].onFailure(err);
                    if (handlers[i].callOnce)
                    {
                        handlers.RemoveAt(i);
                        i--;
                    }
                }
            }

        }

        protected override void OnGameStart()
        {
            Singleton = this; //TODO: Implement on game end and thus better singleton management.
        }

        public struct AcknowledgementHandle
        {
            public Action onSuccess;
            public Action<string> onFailure;
            public bool callOnce;
        }

        public AcknowledgementHandle RegisterHandle(ushort id, Action onSuccess, Action<string> onFailure, bool callOnce = true)
        {
            if (!idToRegisteredHandles.ContainsKey(id))
            {
                idToRegisteredHandles.Add(id, new List<AcknowledgementHandle>());
            }

            var handle = new AcknowledgementHandle
            {
                callOnce = callOnce,
                onSuccess = onSuccess,
                onFailure = onFailure
            };

            idToRegisteredHandles[id].Add(handle);
            return handle;
        }

        public bool RemoveHandle(ushort id, AcknowledgementHandle handle)
        {
            if (!idToRegisteredHandles.ContainsKey(id))
                return false;

            return idToRegisteredHandles[id].Remove(handle);
        }
    }


}
