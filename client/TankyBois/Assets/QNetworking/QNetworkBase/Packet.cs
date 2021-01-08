using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QNetwork
{

    //TODO consider getting rid of movereadpos optional bool
    public class Packet : IDisposable
    {
        public byte[] PacketBuffer { get; private set; }
        private int readWritePos;

        public ushort ID { get; private set; }
        public ushort ExpectedLength { get; private set; }

        public PacketDataType DataType { get; private set; }

        private void InitializePacketWithID(ushort _id)
        {
            DataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(_id);
            readWritePos = 0;
            PacketBuffer = new byte[DataType.MinimumByteLength];
            Write(_id); // Write packet id to the buffer
            Write(DataType.MinimumByteLength);
            ExpectedLength = DataType.MinimumByteLength;
            ID = _id;
        }

        private void InitializePacketWithIDAndStrings(ushort _id, string[] strings)
        {
            DataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(_id);
            readWritePos = 0;

            ushort strlen = 0;

            for (int i = 0; i < strings.Length; i++)
            {
                strlen += (ushort)strings[i].Length;
            }

            PacketBuffer = new byte[DataType.MinimumByteLength + strlen];
            Write(_id); // Write packet id to the buffer
            ExpectedLength = (ushort)(DataType.MinimumByteLength + strlen);
            Write(ExpectedLength);
            ID = _id;

            Write(strings);
        }

        /// <summary>NEED TO USE OTHER CONSTRUCTOR TO CREATE PACKETS THAT CAN SEND STRINGS!! Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(ushort _id)
        {
            InitializePacketWithID(_id);
        }

        /// <summary>Creates a new packet with a given ID and required strings. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(ushort _id, string[] strings)
        {
            InitializePacketWithIDAndStrings(_id, strings);
        }

        public Packet PacketByShallowCopy()
        {
            Packet returned = new Packet();
            returned.PacketBuffer = PacketBuffer;
            returned.DataType = DataType;
            returned.ExpectedLength = ExpectedLength;
            returned.ID = ID;
            returned.readWritePos = readWritePos;
            return returned;
        }

        private Packet()
        {
            
        }

        public struct PacketReadArgs
        {
            public byte[] _data;
            public ushort _id;
            public ushort bufferLen;
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving. The passed byte array must at least contain the initial ID and length ushorts </summary>
        public Packet(byte[] _data)
        {
            readWritePos = 0;
            ExpectedLength = BitConverter.ToUInt16(_data, 2);
            PacketBuffer = new byte[ExpectedLength];
            ID = BitConverter.ToUInt16(_data, 0);
            DataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(ID);
            Write(_data);
            readWritePos = 4;
        }

        #region Utility Functions

        public ushort Length { get { return (ushort)PacketBuffer.Length;} }

        public ushort UnreadLength { get { return (ushort)(Length - readWritePos); } }


        /// <summary>Resets the packet instance to allow it to be reused. An ID is always mandatory for a packet. </summary>
        public void Reset(ushort newID)
        {
            InitializePacketWithID(newID);
        }

        /// <summary>Resets the packet instance to allow it to be reused. An ID is always mandatory for a packet, and strict string rules apply. </summary>
        public void Reset(ushort newID, string[] strings)
        {
            InitializePacketWithIDAndStrings(newID, strings);
        }

        public void SetReadWritePos(ushort newPos)
        {
            readWritePos = newPos;
        }

        #endregion

        #region Write Data
        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="_value">The byte to add.</param>
        public void Write(byte _value)
        {
            PacketBuffer[readWritePos] = _value;
            readWritePos++;
        }
        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="_value">The byte array to add.</param>
        public void Write(byte[] _value)
        {
            Buffer.BlockCopy(_value, 0, PacketBuffer, readWritePos, _value.Length);
            readWritePos += _value.Length;
        }
        /// <summary>Adds a short to the packet.</summary>
        /// <param name="_value">The short to add.</param>
        public void Write(short _value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_value), 0, PacketBuffer, readWritePos, sizeof(short));
            readWritePos += sizeof(short);
        }
        public void Write(ushort _value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_value), 0, PacketBuffer, readWritePos, sizeof(ushort));
            readWritePos += sizeof(ushort);
        }

        /// <summary>Adds an int to the packet.</summary>
        /// <param name="_value">The int to add.</param>
        public void Write(int _value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_value), 0, PacketBuffer, readWritePos, sizeof(int));
            readWritePos += sizeof(int);
        }
        /// <summary>Adds a long to the packet.</summary>
        /// <param name="_value">The long to add.</param>
        public void Write(long _value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_value), 0, PacketBuffer, readWritePos, sizeof(long));
            readWritePos += sizeof(long);
        }
        /// <summary>Adds a float to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(float _value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_value), 0, PacketBuffer, readWritePos, sizeof(float));
            readWritePos += sizeof(float);
        }
        /// <summary>Adds a double to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(double _value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_value), 0, PacketBuffer, readWritePos, sizeof(double));
            readWritePos += sizeof(double);
        }
        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="_value">The bool to add.</param>
        public void Write(bool _value)
        {
            PacketBuffer[readWritePos] = Convert.ToByte(_value);
            readWritePos += sizeof(bool);
        }

        /// <summary>Adds all the strings to the packet. The strings must be added in one go.</summary>
        /// <param name="_value">The string to add.</param>
        private void Write(string[] _values)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                Write((ushort)_values[i].Length);
                Buffer.BlockCopy(Encoding.ASCII.GetBytes(_values[i]), 0, PacketBuffer, readWritePos, _values[i].Length);
            }

        }

        #endregion

        #region Read Data
        /// <summary>Reads a byte from the packet.</summary>
        public byte ReadByte()
        {
            if (PacketBuffer.Length > readWritePos)
            {
                byte _value = PacketBuffer[readWritePos];
                readWritePos += sizeof(byte);
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Creates a new array of bytes consisting of length bytes of the packet and returns it.</summary>
        /// <param name="_length">The length of the byte array.</param>
        public byte[] ReadBytes(int _length)
        {
            var returned = new byte[_length];
            Buffer.BlockCopy(PacketBuffer, readWritePos, returned, 0, _length);
            return returned;
        }

        /// <summary>Reads a short from the packet.</summary>
        public short ReadShort()
        {
            short _value = BitConverter.ToInt16(PacketBuffer, readWritePos);
            readWritePos += sizeof(short);
            return _value;

        }

        /// <summary>Reads a short from the packet.</summary>
        public ushort ReadUShort()
        {
            ushort _value = BitConverter.ToUInt16(PacketBuffer, readWritePos);
            readWritePos += sizeof(ushort);
            return _value;
        }

        /// <summary>Reads an int from the packet.</summary>
        public int ReadInt()
        {
            int _value = BitConverter.ToInt32(PacketBuffer, readWritePos);
            readWritePos += sizeof(int);
            return _value;
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong()
        {
            long _value = BitConverter.ToInt64(PacketBuffer, readWritePos);
            readWritePos += sizeof(long);
            return _value;
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat()
        {
            float _value = BitConverter.ToSingle(PacketBuffer, readWritePos); // Convert the bytes to a float
            readWritePos += sizeof(float);
            return _value;
        }

        /// <summary>Reads a double from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public double ReadDouble()
        {
            double _value = BitConverter.ToDouble(PacketBuffer, readWritePos); // Convert the bytes to a float
            readWritePos += sizeof(float);
            return _value;
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool()
        {
            bool _value = BitConverter.ToBoolean(PacketBuffer, readWritePos); // Convert the bytes to a bool
            readWritePos += sizeof(bool);
            return _value;
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString()
        {
            ushort _length = ReadUShort();
            string _value = Encoding.ASCII.GetString(PacketBuffer, readWritePos, _length); //TODO: ASCII OR UTF-8 DECIDE!
            readWritePos += _length;
            return _value;
        }

        #region Other Read Utilities

        public static Expression GetReaderExpression(TypeCode t, ParameterExpression packetExpression)
        {
            switch (t)
            {
                case TypeCode.String:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadString"));
                case TypeCode.Boolean:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadBool"));
                case TypeCode.Double:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadDouble"));
                case TypeCode.Single:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadFloat"));
                case TypeCode.Int64:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadLong"));
                case TypeCode.Int32:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadInt"));
                case TypeCode.Int16:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadShort"));
                case TypeCode.UInt16:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadUShort"));
                case TypeCode.Byte:
                    return Expression.Call(packetExpression, typeof(Packet).GetMethod("ReadByte"));
            }
            return null;
        }

        /// <summary>
        /// Do remember that boxing and unboxing is not very efficient. This Read function should be used sparingly, if at all.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public object Read(TypeCode t)
        {
            switch (t)
            {
                case TypeCode.String:
                    return ReadString();
                case TypeCode.Boolean:
                    return ReadBool();
                case TypeCode.Double:
                    return ReadDouble();
                case TypeCode.Single:
                    return ReadFloat();
                case TypeCode.Int64:
                    return ReadLong();
                case TypeCode.Int32:
                    return ReadInt();
                case TypeCode.Int16:
                    return ReadShort();
                case TypeCode.UInt16:
                    return ReadUShort();
                case TypeCode.Byte:
                    return ReadByte();
            }
            return null;
        }

        /// <summary>
        /// Reads the desired type and returns it using ToString(). A switch statement is used so this is not as fast.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string ReadAsString(TypeCode t)
        {
            switch (t)
            {
                case TypeCode.String:
                    return ReadString();
                case TypeCode.Boolean:
                    return ReadBool().ToString();
                case TypeCode.Double:
                    return ReadDouble().ToString();
                case TypeCode.Single:
                    return ReadFloat().ToString();
                case TypeCode.Int64:
                    return ReadLong().ToString();
                case TypeCode.Int32:
                    return ReadInt().ToString();
                case TypeCode.Int16:
                    return ReadShort().ToString();
                case TypeCode.UInt16:
                    return ReadShort().ToString();
                case TypeCode.Byte:
                    return ReadByte().ToString();
            }
            return null;
        }

        #endregion

        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    PacketBuffer = null;
                    readWritePos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

