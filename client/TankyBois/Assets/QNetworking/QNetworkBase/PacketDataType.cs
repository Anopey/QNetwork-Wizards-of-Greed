using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace QNetwork
{
    [CreateAssetMenu(fileName = "PacketDataType", menuName = "QNetwork/PacketDataType", order = 1)]
    public class PacketDataType : ScriptableObject
    {

        public string Description;

        public ushort ID;

        [SerializeField]
        private List<PacketHandler> Handlers;

        public TypeCode[] Primitives;

        private ushort calculatedMinimumTotalByteLength = ushort.MaxValue;

        private ushort lengthToTellRealLength;

        private Action<Packet> packetRecieved;

        private void OnEnable()
        {
            if(Primitives == null)
            {
                if (Application.isPlaying)
                {
                    Debug.LogError("Packet Data Type with ID " + ID.ToString() + " does not have a primitives List!");
                }
                return;
            }
            CalculateMinimumByteLength();
            VerifyStringPrimitivePositions();
            VerifyPrepareHandlers();
        }

        #region Initialization

        /// <summary>
        /// Should only be called once during play mode. Called automatically by Packet Manager after it has been initialized by NetworkManager.
        /// </summary>
        public void InitializeDefaultHandlers()
        {
            foreach(var handler in Handlers)
            {
                handler.Initialize();
            }
        }

        private void CalculateMinimumByteLength()
        {
            calculatedMinimumTotalByteLength = 4; //since we start with ID and length.
            foreach (TypeCode t in Primitives)
            {
                switch (t)
                {
                    case TypeCode.Boolean:
                        calculatedMinimumTotalByteLength += 1;
                        break;
                    case TypeCode.Char:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    case TypeCode.SByte:
                        calculatedMinimumTotalByteLength += 1;
                        break;
                    case TypeCode.Byte:
                        calculatedMinimumTotalByteLength += 1;
                        break;
                    case TypeCode.Int16:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    case TypeCode.UInt16:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    case TypeCode.Int32:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.UInt32:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.Int64:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.UInt64:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.Single:
                        calculatedMinimumTotalByteLength += 4;
                        break;
                    case TypeCode.Double:
                        calculatedMinimumTotalByteLength += 8;
                        break;
                    case TypeCode.String:
                        calculatedMinimumTotalByteLength += 2;
                        break;
                    default:
                        Debug.LogError("Unhandled type code " + t.ToString() + " during packet data type length calculation");
                        break;
                }

            }
        }

        private void VerifyStringPrimitivePositions()
        {
            bool flag = false;
            for (ushort i = 0; i < Primitives.Length; i++)
            {
                if (Primitives[i] == TypeCode.String && flag)
                {
                    Debug.LogError("The packet specification of ID " + ID.ToString() + " has a string primtive that is not at the start or right after another string!");
                }
                else
                {
                    flag = true;
                }
            }
        }

        private void VerifyPrepareHandlers()
        {
            foreach (var handler in Handlers)
            {

                VerifyPrepareHandler(handler);

            }
        }

        #endregion

        #region Handler Management and Registration

        private void VerifyPrepareHandler(PacketHandler handler)
        {
            if (handler is ISinglePacketTypeHandler stat)
            {
                for (int i = 0; i < stat.ExpectedPrimitives.Count; i++)
                {
                    if (stat.ExpectedPrimitives[i] != Primitives[i])
                    {
                        Debug.LogError("Static Packet Data Type and Handler Mismatch!\n Data Type: \n" + this.ToString() + " \n Handler:\n" + handler.ToString());
                    }
                }
                packetRecieved += stat.ProcessPacket;
            }
            else if (handler is IMultiplePacketTypeHandler dyn)
            {
                var callback = dyn.VerifyInitializePacket(Primitives);
                if (callback == null)
                {
                    Debug.LogError("Dynamic Packet Data Type and Handler Mismatch!\n Data Type: \n" + this.ToString() + " \n Handler:\n" + handler.ToString());
                }
                packetRecieved += callback;
            }
            else
            {
                Debug.LogError("A packet handler must implement one of the packet handler interfaces!\n Data Type: \n" + this.ToString() + " \n Handler:\n" + handler.ToString());
            }
        }

        public void RegisterHandler(PacketHandler handler)
        {
            VerifyPrepareHandler(handler);
        }

        public void RegisterHandler(Action<Packet> act)
        {
            packetRecieved += act;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// returns empty string if no error.
        /// </summary>
        /// <returns></returns>
        public string VerifyPacket(Packet p)
        {
            try
            {
                p.SetReadWritePos(0);
                p.ReadUShort(); //ID
                p.ReadUShort(); //length
                foreach(var primitive in Primitives)
                {
                    p.Read(primitive);
                }
                p.SetReadWritePos(0);
                return "";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        #endregion

        /// <summary>
        /// This is called minimum since a string can be indefinitely long.
        /// </summary>
        public ushort MinimumByteLength { get { return calculatedMinimumTotalByteLength; } }

        public void OnPacketRecieved(Packet p)
        {
            packetRecieved?.Invoke(p);
        }

    }

}

