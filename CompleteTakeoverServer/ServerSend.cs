using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Service;

namespace CompleteTakeoverServer
{
    class ServerSend
    {
        #region TCP

        static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].tcp.SendData(packet);
        }

        static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }

        static void SendTCPDataToAll(Packet packet, int exceptClient)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i == exceptClient) continue;
                Server.clients[i].tcp.SendData(packet);
            }
        }

        #endregion

        #region UDP

        static void SendUDPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.clients[toClient].udp.SendData(packet);
        }

        static void SendUDPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }

        static void SendUDPataToAll(Packet packet, int exceptClient)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i == exceptClient) continue;
                Server.clients[i].udp.SendData(packet);
            }
        }

        #endregion

        #region Packets

        public static void Welcome(int toClient, string message)
        {
            using(var packet = new Packet((int)ServerPackets.welcomeFromServer))
            {
                packet.Write(message);
                packet.Write(toClient);

                SendTCPData(toClient, packet);
            }
        }

        #region Account

        public static void ConfirmRegister(int toClient, bool confirmed)
        {
            using(var packet = new Packet((int)ServerPackets.registerConfirm))
            {
                packet.Write(confirmed);

                SendTCPData(toClient, packet);
            }
        }

        public static void LoadPlayer(int toClient, PlayerData playerData)
        {
            using(var packet = new Packet((int)ServerPackets.loadPlayer))
            {
                packet.Write(playerData);

                SendTCPData(toClient, packet);
            }
        }

        #endregion

        public static void BuildingAdded(int toClient, int oldID, int newID)
        {
            using(var packet = new Packet((int)ServerPackets.buildingAdded))
            {
                packet.Write(oldID);
                packet.Write(newID);

                SendTCPData(toClient, packet);

                //Console.WriteLine($"Building with ID {oldID} became building with ID {newID}");
            }
        }

        #region Give Base To Attack

        public static void GiveFewRandomBases(int toClient, IEnumerable<int> ids)
        {
            using(var packet = new Packet((int)ServerPackets.giveFewRandomBases))
            {
                packet.Write(ids.Count());
                foreach (int id in ids) packet.Write(id);

                SendTCPData(toClient, packet);
            }
        }

        public static void GiveBaseToAttackData(int toClient, int baseID)
        {
            using(var packet = new Packet((int)ServerPackets.giveAttackableBaseData))
            {
                using(var provider = new ServiceProvider())
                {
                    var data = provider.BaseDataService.Get(baseID);
                    packet.Write(data.ID);
                    packet.Write(data.Player.Username); //for checking
                    packet.Write(data.Player.Faction);
                    packet.Write(data.IsHome);

                    //packet.Write(provider.BaseDataService.GetTotalStoredGold(baseID));
                    //packet.Write(provider.BaseDataService.GetTotalStoredElixir(baseID));

                    packet.Write(data.Buildings.Count);
                    foreach (var building in data.Buildings) packet.Write(building);

                    SendTCPData(toClient, packet);
                }   
            }
        }

        #endregion

        /*
        public static void GivePreloadUpdate(int toClient, int baseID)
        {
            using(var packet = new Packet((int)ServerPackets.givePreloadUpdate))
            {
                using (var provider = new ServiceProvider())
                {
                    var _base = provider.BaseDataService.Get(baseID);
                    DateTime now = DateTime.Now, last = _base.LastVisited;
                    TimeSpan delta = now - last;
                    var mines = provider.MineService.Find(m => m.BaseDataID == baseID);
                    foreach(var mine in mines)
                    {
                        
                    }
                    provider.SaveChanges();
                }
                SendTCPData(toClient, packet);
            }
        }
        */

        public static void UpdateResources(int toClient, int baseID)
        {
            using (var packet = new Packet((int)ServerPackets.updateResources))
            {
                packet.Write(baseID);

                using (var provider = new ServiceProvider())
                {
                    //var _base = provider.BaseDataService.Get(baseID);
                    var hall = provider.MainHallService.Find(h => h.BaseDataID == baseID).First();
                    packet.Write(hall.StoredGold);
                    packet.Write(hall.StoredElixir);
                    var mines = provider.MineService.Find(m => m.BaseDataID == baseID);
                    packet.Write(mines.Count());
                    foreach (var mine in mines)
                        packet.Write(new KeyValuePair<int, int>(mine.ID, mine.Stored));
                    var storages = provider.StorageService.Find(s => s.BaseDataID == baseID);
                    packet.Write(storages.Count());
                    foreach (var storage in storages)
                        packet.Write(new KeyValuePair<int, int>(storage.ID, storage.Stored));
                }

                SendTCPData(toClient, packet);
            }
        }

        public static void UpdateGems(int toClient, string username)
        {
            using (var packet = new Packet((int)ServerPackets.updateGems))
            {
                using (var provider = new ServiceProvider())
                {
                    var player = provider.PlayerDataService.Get(username);
                    int gems = player.Gems;

                    packet.Write(gems);
                }

                SendTCPData(toClient, packet);
            }
        }

        public static void GiveUpdatedPlayerData(int toClient, string username)
        {
            using(var packet = new Packet((int)ServerPackets.giveUpdatedPlayerData))
            {
                using (var provider = new ServiceProvider())
                {
                    var player = provider.PlayerDataService.Get(username);
                    packet.Write(player);
                }

                SendTCPData(toClient, packet);
            }
        }

        #endregion
    }
}
