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

        public TypeCode[] expectedPrimitives = new TypeCode[]
        {
            TypeCode.UInt16,
            TypeCode.String
        };

        public ReadOnlyCollection<TypeCode> ExpectedPrimitives { get { return new ReadOnlyCollection<TypeCode>(expectedPrimitives); } }

        public void ProcessPacket(Packet p)
        {

        }

        protected override void OnGameStart()
        {
            Singleton = this; //TODO: Implement on game end and thus better singleton management.
        }


    }
}
