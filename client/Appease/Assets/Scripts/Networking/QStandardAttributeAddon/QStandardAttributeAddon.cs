using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace Game.QNetwork.StandardAttributeAddon
{

    using QNetwork;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Standard Attribute Addon", menuName = "QNetwork/Addons/StandardAttributeAddon", order = 1)]
    public class QStandardAttributeAddon : QAddon
    {
        protected override void OnNetworkManagerInitialized()
        {
            Debug.Log("QNetworking standard attribute addon is now being initialized...");

            //very very expensive operation but only run once.

            var allTypes = AppDomain.CurrentDomain.GetAssemblies().
            SelectMany(s => s.GetTypes());

            var candidateMethods = allTypes.
                SelectMany(i => i.GetMethods()).
                Where(m => Attribute.IsDefined(m, typeof(QDataReciever)));

            foreach(var candidate in candidateMethods)
            {
                if (!candidate.IsStatic)
                {
                    Debug.LogError(candidate.Name + " is trying to be used as a QDataReciever but is not static!");
                    continue;
                }

                var attribute = (QDataReciever)candidate.GetCustomAttribute(typeof(QDataReciever));

                Type[] primitives = candidate.GetParameters().Select(i => i.ParameterType).ToArray();
                ushort id = attribute.PacketID;

                if (!NetworkManager.Singleton.PacketManager.IDIsValid(id))
                {
                    Debug.LogError(candidate.Name + " has an invalid packet ID!");
                    continue;
                }

                var packetDataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(id);

                var packetPrimitives = packetDataType.Primitives;

                if(packetPrimitives.Length != primitives.Length)
                {
                    Debug.LogError(candidate.Name + " does not accept the same primitives as the packet type specified by its ID!");
                    continue;
                }

                for(int i = 0; i < packetPrimitives.Length; i++)
                {
                    if(packetPrimitives[i] != Type.GetTypeCode(primitives[i]))
                    {
                        Debug.LogError(candidate.Name + " does not accept the same primitives as the packet type specified by its ID!");
                        continue;
                    }
                }

                //now we may zubzcrayb

                ParameterExpression packetExpression = ParameterExpression.Parameter(typeof(Packet));

                List<Expression> expressions = new List<Expression>();

                foreach(var prim in packetPrimitives)
                {
                    expressions.Add(Packet.GetReaderExpression(prim, packetExpression));
                }

                var handler = Expression.Lambda<Action<Packet>>(Expression.Call(candidate, expressions), packetExpression).Compile();

                packetDataType.RegisterHandler(handler);
            }
        }
    }
}
