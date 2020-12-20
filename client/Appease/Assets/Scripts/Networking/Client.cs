using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

namespace Game.Networking
{

    public class Client : MonoBehaviour
    {

        public static readonly int dataBufferSize = 4096;


        private TCP _tcp;
        private string _ip = "127.0.0.1";
        private int _port = 52515;
        private int _id = 0;


        public TCP TCPInstance { get { return _tcp; } }
        public string ServerIP { get { return _ip; } }
        public int Port { get { return _port; } }
        public int ID { get { return _id; } }

        #region Singleton Architecture

        public static Client Singleton { get; private set; }

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(this);
                return;
            }
            Singleton = this;
        }

        private void OnDestroy()
        {
            if (Singleton == this)
                Singleton = null;
        }


        #endregion

        private void Start()
        {
            _tcp = new TCP();
        }


        public void ConnectToServer()
        {
            TCPInstance.Connect();
        }

        public void WriteToServer(Packet packet)
        {
            TCPInstance.SendPacket(packet);
        }

        public class TCP
        {
            public TcpClient socket { get; private set; }

            private NetworkStream stream;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(Singleton._ip, Singleton.Port, ConnectCallback, socket);
            }

            public void SendPacket(Packet packet)
            {
                try
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
                catch(Exception e)
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
                }

                stream = socket.GetStream();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
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

                    //TODO: Handle Data

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error during read from server: \n" + e.Message);
                    //TODO: Disconnect
                }
            }
        }
    }

}