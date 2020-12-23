using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Networking
{

    //This class was initially imported from Tom Weiland's C# Networking Series. You may find them at https://www.youtube.com/watch?v=4uHTSknGJaY&list=PLXkn83W0QkfnqsK8I0RAz5AbUxfg3bOQ5&index=2

    //TODO consider getting rid of movereadpos optional bool
    public class Packet : IDisposable
    {
        private List<byte> bufferList;
        private byte[] bufferArray;
        private int readPos;

        public ushort ID { get; private set; }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(ushort _id)
        {
            bufferList = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            Write(_id); // Write packet id to the buffer
            ID = _id;
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving. Also automatically sets the ID of the packet. </summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public Packet(byte[] _data)
        {
            bufferList = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            ID = ReadUShort(true);

            SetBytes(_data);
        }

        #region Functions
        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public void SetBytes(byte[] _data)
        {
            bufferList.Clear();
            Write(_data);
            bufferArray = bufferList.ToArray();
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            bufferArray = bufferList.ToArray();
            return bufferArray;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return bufferList.Count; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - readPos; // Return the remaining length (unread)
        }

        /// <summary>Resets the packet instance to allow it to be reused. An ID is always mandatory for a packet. </summary>
        public void Reset(int newID)
        {
            bufferList.Clear(); // Clear buffer
            bufferArray = null;
            readPos = 0; // Reset readPos

            Write(newID);
        }
        #endregion

        #region Write Data
        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="_value">The byte to add.</param>
        public void Write(byte _value)
        {
            bufferList.Add(_value);
        }
        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="_value">The byte array to add.</param>
        public void Write(byte[] _value)
        {
            bufferList.AddRange(_value);
        }
        /// <summary>Adds a short to the packet.</summary>
        /// <param name="_value">The short to add.</param>
        public void Write(short _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }
        public void Write(ushort _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }

        /// <summary>Adds an int to the packet.</summary>
        /// <param name="_value">The int to add.</param>
        public void Write(int _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a long to the packet.</summary>
        /// <param name="_value">The long to add.</param>
        public void Write(long _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a float to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(float _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a double to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(double _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="_value">The bool to add.</param>
        public void Write(bool _value)
        {
            bufferList.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a string to the packet.</summary>
        /// <param name="_value">The string to add.</param>
        public void Write(string _value)
        {
            Write(_value.Length); // Add the length of the string to the packet
            bufferList.AddRange(Encoding.ASCII.GetBytes(_value)); // Add the string itself
        }

        #endregion

        #region Read Data
        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (bufferList.Count > readPos)
            {
                byte _value = bufferArray[readPos];
                if (_moveReadPos)
                {
                    readPos += sizeof(byte);
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="_length">The length of the byte array.</param>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (bufferList.Count > readPos)
            {
                byte[] _value = bufferList.GetRange(readPos, _length).ToArray();
                if (_moveReadPos)
                {
                    readPos += _length;
                }
                return _value;
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
            if (bufferList.Count > readPos)
            {
                short _value = BitConverter.ToInt16(bufferArray, readPos);
                if (_moveReadPos)
                {
                    readPos += sizeof(short);
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
            if (bufferList.Count > readPos)
            {
                ushort _value = BitConverter.ToUInt16(bufferArray, readPos);
                if (_moveReadPos)
                {
                    readPos += sizeof(ushort);
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
            if (bufferList.Count > readPos)
            {
                int _value = BitConverter.ToInt32(bufferArray, readPos);
                if (_moveReadPos)
                {
                    readPos += sizeof(int);
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
            if (bufferList.Count > readPos)
            {
                long _value = BitConverter.ToInt64(bufferArray, readPos);
                if (_moveReadPos)
                {
                    readPos += sizeof(long);
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
            if (bufferList.Count > readPos)
            {
                float _value = BitConverter.ToSingle(bufferArray, readPos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    readPos += sizeof(float);
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
            if (bufferList.Count > readPos)
            {
                double _value = BitConverter.ToDouble(bufferArray, readPos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    readPos += sizeof(float);
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
            if (bufferList.Count > readPos)
            {
                bool _value = BitConverter.ToBoolean(bufferArray, readPos); // Convert the bytes to a bool
                if (_moveReadPos)
                {
                    readPos += sizeof(bool);
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
                int _length = ReadInt(); // Get the length of the string
                string _value = Encoding.ASCII.GetString(bufferArray, readPos, _length); // Convert the bytes to a string
                if (_moveReadPos && _value.Length > 0)
                {
                    // If _moveReadPos is true string is not empty
                    readPos += _length; // Increase readPos by the length of the string
                }
                return _value; // Return the string
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
                    bufferList = null;
                    bufferArray = null;
                    readPos = 0;
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

