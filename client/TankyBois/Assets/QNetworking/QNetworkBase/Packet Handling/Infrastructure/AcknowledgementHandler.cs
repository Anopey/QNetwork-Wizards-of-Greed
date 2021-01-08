using System;
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

        public static AcknowledgementHandler Singleton { get; private set; }

        private Dictionary<ushort, List<AcknowledgementHandle>> idToRegisteredHandles = new Dictionary<ushort, List<AcknowledgementHandle>>();

        public TypeCode[] expectedPrimitives = new TypeCode[]
        {
            TypeCode.UInt16,
            TypeCode.String
        };

        public ReadOnlyCollection<TypeCode> ExpectedPrimitives { get { return new ReadOnlyCollection<TypeCode>(expectedPrimitives); } }

        public void ProcessPacket(Packet p)
        {
            string err = p.ReadString();
            ushort id = p.ReadUShort();
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
