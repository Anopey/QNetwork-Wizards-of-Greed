using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;


namespace QNetwork
{

    public class Client
    {

        public static readonly ushort dataBufferSize = 4096;


        private TCP _tcp;
        private string _ip = "127.0.0.1";
        private int _port = 52515;
        private int _id = 0;


        public TCP TCPInstance { get { return _tcp; } }
        public string ServerIP { get { return _ip; } }
        public int Port { get { return _port; } }
        public int ID { get { return _id; } }

        #region Singleton Architecture

        private static Client _singleton;

        public static Client Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    Singleton = new Client();
                    Singleton._tcp = new TCP();
                }
                return _singleton;
            }
            private set
            {
                _singleton = value;
            }
        }

        #endregion


        public void ConnectToServer(Action callback)
        {
            TCPInstance.Connect(callback);
        }

        public void WriteToServer(ushort packetID, Packet packet)
        {
#if UNITY_EDITOR 
            if (!NetworkManager.Singleton.PacketManager.IDIsValid(packetID))
            {
                Debug.LogError("During write: \nPacket ID " + packetID.ToString() + " does not exist!");
                return;
            }

            PacketDataType dataType = NetworkManager.Singleton.PacketManager.GetPacketDataFromID(packetID);

            string err = dataType.VerifyPacket(packet);

            if (err != "")
            {
                Debug.LogError("During write: \n" + err);
                return;
            }
#endif
            TCPInstance.SendPacket(packet);
        }

        public class TCP
        {
            public TcpClient socket { get; private set; }

            private NetworkStream stream;
            private byte[] receiveBuffer;

            private Packet recievedData;

            private Action onConnectSingleCallback;

            public void Connect(Action OnConnect)
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];

                onConnectSingleCallback = OnConnect;

                socket.BeginConnect(Singleton._ip, Singleton.Port, ConnectCallback, socket);
            }

            public void SendPacket(Packet packet)
            {
                try
                {
                    stream.BeginWrite(packet.PacketBuffer, 0, packet.Length, null, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error during writing to server: \n" + e.Message);
                }
            }

            private void ConnectCallback(IAsyncResult result)
            {
                socket.EndConnect(result);

                if (!socket.Connected)
                {
                    Debug.LogError("Failed to connect!");
                    return;
                }

                stream = socket.GetStream();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                NetworkManager.ExecuteOnMainThread(() =>
               {
                   onConnectSingleCallback?.Invoke();
                   onConnectSingleCallback = null;
               });
            }

            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);

                    if (byteLength <= 0)
                    {
                        Debug.LogError("Read byte length invalid!");
                        //TODO: Disconnect
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);

                    stream.BeginRead(receiveBuffer, 0, HandleData(data), ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error during read from server: \n" + e.Message);
                    //TODO: Disconnect
                }
            }

            private ushort HandleData(byte[] _data) //TODO: Optimize even further if needed!
            {
                if (recievedData == null)
                {
                    //Start reading a new packet.
                    if (_data.Length < 4)
                        Debug.LogError("Now we know that we actually can get lower than 4 bytes at the start of a new packet. Readjust accordingly!");

                    recievedData = new Packet(_data);
                }
                else
                {
                    //ongoing packet

                    //if (recievedData.ExpectedLength < recievedData.Length + _data.Length)
                    //    Debug.LogError("Ok so a packet and the start of another packet can get mixed up it seems. Readjust accordingly!");

                    recievedData.Write(_data);
                }


                if (recievedData.Length == recievedData.ExpectedLength)
                {
                    //our packet is now complete!

                    Packet p = recievedData.PacketByShallowCopy();
                    recievedData = null;
                    NetworkManager.ExecuteOnMainThread(() =>
                    {
                        p.DataType.OnPacketRecieved(p);
                    });
                }
                else
                {
                    return (ushort)(recievedData.ExpectedLength - recievedData.Length);
                }

                return dataBufferSize;
            }
        }
    }

}