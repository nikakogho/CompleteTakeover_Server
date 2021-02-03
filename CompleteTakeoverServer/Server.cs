using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CompleteTakeover.Service;
using CompleteTakeover.Domain;

namespace CompleteTakeoverServer
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static bool IsPlayerOnline(PlayerData player)
        {
            if (player == null) return false;
            foreach (var client in clients) if (client.Value.player == player) return true;
            return false;
        }

        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;   
            Port = port;

            Console.WriteLine("Starting...");

            InitServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            udpListener = new UdpClient(Port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            using(var provider = new ServiceProvider())
            {
                provider.SaveChanges();
            }

            Console.WriteLine($"Started on {Port}");
        }

        static void TCPConnectCallback(IAsyncResult result)
        {
            var client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if(clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server is full!");
        }

        static void UDPReceiveCallback(IAsyncResult result)
        {
            try
            {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = udpListener.EndReceive(result, ref clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (data.Length < 4) return;

                using (var packet = new Packet(data))
                {
                    int clientID = packet.ReadInt();

                    if (clientID == 0) return;

                    if(clients[clientID].udp.endPoint == null)
                    {
                        clients[clientID].udp.Connect(clientEndPoint);
                        return;
                    }

                    if(clients[clientID].udp.endPoint.ToString() == clientEndPoint.ToString())
                    {
                        clients[clientID].udp.HandleData(packet);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error receiving UDP data: {e}");
            }
        }

        public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
        {
            try
            {
                if(clientEndPoint != null)
                {
                    udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error sending data to {clientEndPoint} via UDP: {e}");
            }
        }
        
        static void InitServerData()
        {
            for(int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.registerCheck, ServerHandle.RegisterRequested },
                { (int)ClientPackets.loadPlayerRequest, ServerHandle.LoginRequested },
                { (int)ClientPackets.addBuilding, ServerHandle.AddBuilding },
                { (int)ClientPackets.upgradeBuilding, ServerHandle.UpgradeBuiling },
                { (int)ClientPackets.buildingFixed, ServerHandle.BuildingFixed },
                { (int)ClientPackets.reportAttack, ServerHandle.ReportAttack },
                { (int)ClientPackets.reportDefense, ServerHandle.ReportDefense },
                { (int)ClientPackets.defenderProcessedCombat, ServerHandle.DefenseProcessed },
                { (int)ClientPackets.requestFewRandomBases, ServerHandle.RequestFewRandomBases },
                { (int)ClientPackets.requestAttackableBaseData, ServerHandle.RequestAttackableBaseData },
                { (int)ClientPackets.addUnit, ServerHandle.AddUnit },
                { (int)ClientPackets.removeUnit, ServerHandle.RemoveUnit },
                { (int)ClientPackets.addResources, ServerHandle.AddResources },
                { (int)ClientPackets.subtractResources, ServerHandle.SubtractResources },
                { (int)ClientPackets.addGems, ServerHandle.AddGems },
                { (int)ClientPackets.subtractGems, ServerHandle.SubtractGems },
                { (int)ClientPackets.moveBuilding, ServerHandle.MoveBuilding },
                { (int)ClientPackets.updateMineStored, ServerHandle.UpdateMineStored },
                { (int)ClientPackets.updateStorageValue, ServerHandle.UpdateStorageStored },
                { (int)ClientPackets.leavingBase, ServerHandle.LeavingBase },
                { (int)ClientPackets.disconnecting, ServerHandle.Disconnected },
                { (int)ClientPackets.addBuilder, ServerHandle.AddBuilder },
                { (int)ClientPackets.requestPlayerDataUpdate, ServerHandle.RequestPlayerDataUpdate }
            };

            Console.WriteLine("Initialized packets.");
        }
    }
}
