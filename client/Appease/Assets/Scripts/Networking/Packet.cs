using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Networking
{

    //TODO consider getting rid of movereadpos optional bool
    public class Packet : IDisposable
    {
        public byte[] PacketBuffer { get; private set; }
        private int readWritePos;

        public ushort ID { get; private set; }

        private PacketDataType packetDataType;

        private void InitializePacketWithID(ushort _id)
        {
            packetDataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(_id);
            readWritePos = 4; 
            PacketBuffer = new byte[packetDataType.MinimumByteLength];
            Write(_id); // Write packet id to the buffer
            Write(packetDataType.MinimumByteLength);
            ID = _id;
        }

        private void InitializePacketWithIDAndStrings(ushort _id, string[] strings)
        {
            packetDataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(_id);
            readWritePos = 4;

            ushort strlen = 0;

            for (int i = 0; i < strings.Length; i++)
            {
                strlen += (ushort)strings[i].Length;
            }

            PacketBuffer = new byte[packetDataType.MinimumByteLength + strlen];
            Write(_id); // Write packet id to the buffer
            Write((ushort)(packetDataType.MinimumByteLength + strlen));
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

        public struct PacketReadArgs
        {
            public byte[] _data;
            public ushort _id;
            public ushort bufferLen;
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving. The passed byte array must at least contain the initial ID and length ushorts </summary>
        public Packet(byte[] _data)
        {
            readWritePos = 4; 
            PacketBuffer = new byte[BitConverter.ToUInt16(_data, 2)];
            ID = BitConverter.ToUInt16(_data, 0);
            packetDataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(ID);
            Write(_data);
        }

        #region Functions

        /// <summary>
        /// Gets the length of the total number of packets this packet will have to contain. Returns 0 if the current information inside packet is not enough to interpret this.
        /// </summary>
        public ushort InterpretReadTotalCompleteLength()
        {
            var dataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(ID);

            if (dataType.StringPrimitveCount == 0)
                return dataType.MinimumByteLength;

            if (Length() - 2 < dataType.StringPrimitveCount * 2)
                return 0;

            ushort len = dataType.MinimumByteLength;
            for(int i = 2; i < dataType.StringPrimitveCount * 2 + 2; i += 2)
            {
                len += BitConverter.ToUInt16(PacketBuffer, i);
            }
            return len;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public ushort Length()
        {
            return (ushort)PacketBuffer.Length; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public ushort UnreadLength()
        {
            return (ushort)(Length() - readWritePos); // Return the remaining length (unread)
        }

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
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                byte _value = PacketBuffer[readWritePos];
                if (_moveReadPos)
                {
                    readWritePos += sizeof(byte);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Creates a new array of bytes consisting of length bytes of the packet and returns it.</summary>
        /// <param name="_length">The length of the byte array.</param>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                var returned = new byte[_length];
                Buffer.BlockCopy(PacketBuffer, readWritePos, returned, 0, _length);
                if (_moveReadPos)
                {
                    readWritePos += _length;
                }
                return returned;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                short _value = BitConverter.ToInt16(PacketBuffer, readWritePos);
                if (_moveReadPos)
                {
                    readWritePos += sizeof(short);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public ushort ReadUShort(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                ushort _value = BitConverter.ToUInt16(PacketBuffer, readWritePos);
                if (_moveReadPos)
                {
                    readWritePos += sizeof(ushort);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                int _value = BitConverter.ToInt32(PacketBuffer, readWritePos);
                if (_moveReadPos)
                {
                    readWritePos += sizeof(int);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                long _value = BitConverter.ToInt64(PacketBuffer, readWritePos);
                if (_moveReadPos)
                {
                    readWritePos += sizeof(long);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                float _value = BitConverter.ToSingle(PacketBuffer, readWritePos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    readWritePos += sizeof(float);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a double from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public double ReadDouble(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                double _value = BitConverter.ToDouble(PacketBuffer, readWritePos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    readWritePos += sizeof(float);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool _moveReadPos = true)
        {
            if (PacketBuffer.Length > readWritePos)
            {
                bool _value = BitConverter.ToBoolean(PacketBuffer, readWritePos); // Convert the bytes to a bool
                if (_moveReadPos)
                {
                    readWritePos += sizeof(bool);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                ushort _length = ReadUShort();
                string _value = Encoding.ASCII.GetString(PacketBuffer, readWritePos, _length); //TODO: ASCII OR UTF-8 DECIDE!
                if (_moveReadPos)
                {
                    readWritePos += _length;
                }
                else
                {
                    readWritePos -= 2; //since we read length.
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }
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

