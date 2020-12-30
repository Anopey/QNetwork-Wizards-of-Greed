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

        [SerializeField]
        private PacketHandler[] Handlers;

        public TypeCode[] Primitives;

        private ushort calculatedMinimumTotalByteLength = ushort.MaxValue;

        private ushort lengthToTellRealLength;

        private Action<Packet> packetRecieved;

        private void Awake()
        {
#if UNITY_EDITOR
            if (Handlers.Length == 0) //no need to bug developer during development.
            {
                return;
            }
#else
            if (Handlers.Length == 0)
            {
                Debug.LogError("Packet data type with ID " + ID.ToString() + " has no handlers assigned!");
            }
#endif
            CalculateMinimumByteLength();
            VerifyStringPrimitivePositions();
            VerifyPrepareHandlers();
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
                //if(handler.ExpectedPrimitives.Length != Primitives.Length)
                //{
                //    Debug.LogError("Packet Data Type and Handler Mismatch!\n Data Type: \n" + this.ToString() + " \n Handler:\n" + handler.ToString());
                //}
                for (int i = 0; i < handler.ExpectedPrimitives.Length; i++)
                {
                    if (handler.ExpectedPrimitives[i] != Primitives[i])
                    {
                        Debug.LogError("Packet Data Type and Handler Mismatch!\n Data Type: \n" + this.ToString() + " \n Handler:\n" + handler.ToString());
                    }
                }

                packetRecieved += handler.OnPacketRecieved;
            }
        }

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

