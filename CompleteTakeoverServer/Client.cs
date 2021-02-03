using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using CompleteTakeover.Domain;

namespace CompleteTakeoverServer
{
    class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;
        public UDP udp;

        public PlayerData player;

        public Client(int id)
        {
            this.id = id;
            tcp = new TCP(id);
            udp = new UDP(id);
        }

        public class TCP
        {
            public TcpClient socket;
            readonly int id;
            NetworkStream stream;
            byte[] receiveBuffer;

            Packet receiveData;

            public TCP(int id)
            {
                this.id = id;
            }

            public void Connect(TcpClient socket)
            {
                this.socket = socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receiveData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(id, "welcome to server!");
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if(socket != null)
                    {
                        stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error sending data to client {id} via TCP: {e.Message}");
                }
            }

            void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    int len = stream.EndRead(result);
                    if(len <= 0)
                    {
                        Server.clients[id].Disconnect();
                        return;
                    }

                    var data = new byte[len];
                    Array.Copy(receiveBuffer, data, len);

                    receiveData.Reset(HandleData(data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Error {e}");
                    Server.clients[id].Disconnect();
                }
            }

            bool HandleData(byte[] data)
            {
                int length = 0;

                receiveData.SetBytes(data);

                if (receiveData.UnreadLength() >= 4)
                {
                    length = receiveData.ReadInt();
                    if (length <= 0) return true;
                }

                while (length > 0 && length <= receiveData.UnreadLength())
                {
                    var packetBytes = receiveData.ReadBytes(length);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (var packet = new Packet(packetBytes))
                        {
                            int packetID = packet.ReadInt();
                            Server.packetHandlers[packetID](id, packet);
                        }
                    });

                    length = 0;
                    if (receiveData.UnreadLength() >= 4)
                    {
                        length = receiveData.ReadInt();
                        if (length <= 0) return true;
                    }
                }

                if (length <= 1) return true;

                return false;
            }

            public void Disconnect()
            {
                socket.Close();
                stream = null;
                receiveData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        public class UDP
        {
            public IPEndPoint endPoint;

            int id;

            public UDP(int id)
            {
                this.id = id;
            }

            public void Connect(IPEndPoint endPoint)
            {
                this.endPoint = endPoint;
            }

            public void SendData(Packet packet)
            {
                Server.SendUDPData(endPoint, packet);
            }

            public void HandleData(Packet packetData)
            {
                int len = packetData.ReadInt();
                var packetBytes = packetData.ReadBytes(len);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using(var packet = new Packet(packetBytes))
                    {
                        int packetID = packet.ReadInt();
                        Server.packetHandlers[packetID](id, packet);
                    }
                });
            }

            public void Disconnect()
            {
                endPoint = null;
            }
        }

        public void Disconnect()
        {
            Console.WriteLine($"{tcp.socket.Client.RemoteEndPoint} has disconnected.");

            tcp.Disconnect();
            udp.Disconnect();
        }
    }
}
