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

        public UnityEvent<Packet> Handler;

        public TypeCode[] Primitives;

        private ushort calculatedMinimumTotalByteLength = ushort.MaxValue;

        /// <summary>
        /// This is called minimum since a string can be indefinitely long.
        /// </summary>
        /// <returns></returns>
        public ushort GetMinimumByteLength()
        {
            if(calculatedMinimumTotalByteLength == ushort.MaxValue)
            {
                calculatedMinimumTotalByteLength = 0;
                foreach(TypeCode t in Primitives)
                {
                    switch (t)
                    {
                        case TypeCode.Object:
                            calculatedMinimumTotalByteLength += 4;
                            break;
                        case TypeCode.DBNull:
                            calculatedMinimumTotalByteLength += 4;
                            break;
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
                        case TypeCode.Decimal:
                            calculatedMinimumTotalByteLength += 16;
                            break;
                        case TypeCode.DateTime:
                            calculatedMinimumTotalByteLength += 8;
                            break;
                        case TypeCode.String:
                            calculatedMinimumTotalByteLength += 4;
                            break;
                        default:
                            Debug.LogError("Unexpected type code during packet data type length calculation");
                            break;
                    }
                }
                
            }
            return calculatedMinimumTotalByteLength;
        }
    }

}

