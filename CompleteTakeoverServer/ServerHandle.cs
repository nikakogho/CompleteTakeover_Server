using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Service;
using CompleteTakeover.Domain;

namespace CompleteTakeoverServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int idCheck = packet.ReadInt();
            string username = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}!");
            if(fromClient != idCheck)
                Console.WriteLine("Something went wrong!");

            //may do something when connecting
        }

        #region Account

        public static void RegisterRequested(int fromClient, Packet packet)
        {
            string username = packet.ReadString();
            string password = packet.ReadString();
            string faction = packet.ReadString();

            using(var provider = new ServiceProvider())
            {
                bool registered = provider.Register(username, password, faction);

                ServerSend.ConfirmRegister(fromClient, registered);
            }
        }

        public static void LoginRequested(int fromClient, Packet packet)
        {
            string username = packet.ReadString();
            int pasLen = packet.ReadInt();
            byte[] password = packet.ReadBytes(pasLen);

            using(var provider = new ServiceProvider())
            {
                var player = provider.PlayerDataService.Login(username, password);
                if (player == null) Console.WriteLine("Player failed to Log In");
                else Console.WriteLine("Player successfully logged in");
                Server.clients[fromClient].player = player;
                ServerSend.LoadPlayer(fromClient, player);
            }
        }

        #endregion

        #region Building

        public static void BuildingFixed(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                provider.GetBuilding(id).Destroyed = false;
                provider.SaveChanges();
            }
        }

        public static void AddBuilding(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();
            var instance = packet.ReadBuildingInstanceData();
            int oldID = instance.ID;
            int newID;

            using(var provider = new ServiceProvider())
            {
                provider.AddBuilding(instance);
                var baseData = provider.BaseDataService.Get(baseID);
                baseData.Buildings.Add(instance);
                provider.SaveChanges();
                newID = instance.ID;
            }

            ServerSend.BuildingAdded(fromClient, oldID, newID);
        }

        public static void MoveBuilding(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            int x = packet.ReadInt();
            int y = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                var building = provider.GetBuilding(id);
                building.TileX = x;
                building.TileY = y;
                provider.SaveChanges();
            }
        }

        public static void UpgradeBuiling(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            string type = packet.ReadString();

            using (var provider = new ServiceProvider())
            {
                var building = provider.GetBuilding(id);
                building.Level++;

                switch (type)
                {
                    case "Storage":
                        var storage = provider.StorageService.Get(id);
                        storage.Capacity = packet.ReadInt();
                        break;

                    case "Main Hall":
                        var hall = provider.MainHallService.Get(id);
                        hall.GoldCapacity = packet.ReadInt();
                        hall.ElixirCapacity = packet.ReadInt();
                        break;
                }

                provider.SaveChanges();
            }
        }

        #endregion

        #region Report

        public static void ReportAttack(int fromClient, Packet packet)
        {
            int attackerBaseID = packet.ReadInt();
            int attackedBaseID = packet.ReadInt();
            string attackerUsername = packet.ReadString();
            bool wasRevenge = packet.ReadBool();

            int takenGold = packet.ReadInt();
            int takenElixir = packet.ReadInt();

            int armySize = packet.ReadInt();
            var squadStuff = new List<SquadData>();
            for (int i = 0; i < armySize; i++)
            {
                var squad = new SquadData()
                {
                    UnitName = packet.ReadString(),
                    Amount = packet.ReadInt()
                };
                squadStuff.Add(squad);
            }

            int destroyedBuildingCount = packet.ReadInt();
            var destroyedBuildingIDs = new List<int>();
            for (int i = 0; i < destroyedBuildingCount; i++) destroyedBuildingIDs.Add(packet.ReadInt());

            int robbedMinesCount = packet.ReadInt();
            var robbedMines = new Dictionary<int, int>();
            for (int i = 0; i < robbedMinesCount; i++) robbedMines.Add(packet.ReadInt(), packet.ReadInt());

            int robbedStoragesCount = packet.ReadInt();
            var robbedStorages = new Dictionary<int, int>();
            for (int i = 0; i < robbedStoragesCount; i++) robbedStorages.Add(packet.ReadInt(), packet.ReadInt());

            using (var provider = new ServiceProvider())
            {
                var attackedBase = provider.BaseDataService.Get(attackedBaseID);
                var attackerBase = provider.BaseDataService.Get(attackerBaseID);

                var destroyedBuildings = new List<BuildingInstanceData>();
                foreach (int id in destroyedBuildingIDs)
                    destroyedBuildings.Add(provider.GetBuilding(id));

                foreach (var robbedMine in robbedMines)
                    provider.MineService.Get(robbedMine.Key).Stored -= robbedMine.Value;

                foreach (var robbedStorage in robbedStorages)
                    provider.StorageService.Get(robbedStorage.Key).Stored -= robbedStorage.Value;

                var attacker = provider.PlayerDataService.Get(attackerUsername);

                int elixirToAdd = attacker.Faction == attackedBase.Player.Faction ? takenElixir : 0;

                provider.SquadDataService.AddRange(squadStuff);
                provider.BaseDataService.AddResources(attackerBaseID, takenGold, elixirToAdd);
                
                var report = new AttackReport
                {
                    Base = attackedBase,
                    Attacker = attacker,
                    WasRevenge = wasRevenge,
                    Avenged = wasRevenge,
                    AmassedGold = takenGold,
                    AmassedElixir = takenElixir,
                    DestroyedBuildings = destroyedBuildings,
                    DeployedArmy = squadStuff
                };

                provider.AttackReportService.Add(report);
                attacker.Attacks.Add(report);
                attackedBase.AttackedByPlayerReports.Add(report);

                foreach (var squad in squadStuff) squad.DeployedInAttack = report;

                provider.SaveChanges();
            }
        }

        public static void RequestPlayerDataUpdate(int fromClient, Packet packet)
        {
            string username = packet.ReadString();

            if (Server.clients[fromClient].player.Username != username) Console.WriteLine("Not the player of this client!");
            else ServerSend.GiveUpdatedPlayerData(fromClient, username);
        }

        public static void ReportDefense(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();
            string invasionName = packet.ReadString();

            int lostGold = packet.ReadInt();
            int lostElixir = packet.ReadInt();

            int armySize = packet.ReadInt();
            var squadStuff = new List<SquadData>();
            for (int i = 0; i < armySize; i++)
            {
                var squad = new SquadData()
                {
                    UnitName = packet.ReadString(),
                    Amount = packet.ReadInt()
                };
                squadStuff.Add(squad);
            }
            
            int destroyedBuildingCount = packet.ReadInt();
            var destroyedBuildingIDs = new List<int>();
            for (int i = 0; i < destroyedBuildingCount; i++) destroyedBuildingIDs.Add(packet.ReadInt());

            int robbedMinesCount = packet.ReadInt();
            var robbedMines = new Dictionary<int, int>();
            for (int i = 0; i < robbedMinesCount; i++) robbedMines.Add(packet.ReadInt(), packet.ReadInt());

            int robbedStoragesCount = packet.ReadInt();
            var robbedStorages = new Dictionary<int, int>();
            for (int i = 0; i < robbedStoragesCount; i++) robbedStorages.Add(packet.ReadInt(), packet.ReadInt());

            using (var provider = new ServiceProvider())
            {
                var _base = provider.BaseDataService.Get(baseID);

                var destroyedBuildings = new List<BuildingInstanceData>();
                foreach (int id in destroyedBuildingIDs)
                {
                    var building = provider.GetBuilding(id);
                    building.Destroyed = true;
                    destroyedBuildings.Add(building);
                }
                
                foreach (var robbedMine in robbedMines)
                    provider.MineService.Get(robbedMine.Key).Stored -= robbedMine.Value;

                foreach (var robbedStorage in robbedStorages)
                    provider.StorageService.Get(robbedStorage.Key).Stored -= robbedStorage.Value;

                provider.SquadDataService.AddRange(squadStuff);

                var report = new DefenseReport()
                {
                    Base = _base,
                    InvasionName = invasionName,
                    LostGold = lostGold,
                    LostElixir = lostElixir,
                    DestroyedBuildings = destroyedBuildings,
                    DeployedArmy = squadStuff
                };
                provider.DefenseReportService.Add(report);
                _base.AttackedByAIReports.Add(report);

                foreach (var squad in squadStuff) squad.DeployedInDefense = report;

                provider.SaveChanges();
            }
        }

        public static void DefenseProcessed(int fromClient, Packet packet)
        {
            bool fromAI = packet.ReadBool();
            int id = packet.ReadInt();

            Console.WriteLine($"Defense with id {id} has been processed");

            using (var provider = new ServiceProvider())
            {
                if (fromAI) provider.DefenseReportService.Get(id).Processed = true;
                else provider.AttackReportService.Get(id).ProcessedOnDefendersSide = true;

                provider.SaveChanges();
            }
        }

        #endregion

        #region Attack Request

        public static void RequestFewRandomBases(int fromClient, Packet packet)
        {
            int count = packet.ReadInt();
            string username = packet.ReadString();
            List<int> options;

            using (var provider = new ServiceProvider())
            {
                var player = provider.PlayerDataService.Get(username);
                options = provider.BaseDataService
                    .Find(b => (b.Player.Username != username))
                    .TakeWhile(data => (count--) > 0)
                    .Select(data => data.ID).ToList();
                for(int i = options.Count - 1; i > -1; i--)
                    if (provider.BaseDataService.IsDestroyed(options[i])) options.RemoveAt(i);
            }

            ServerSend.GiveFewRandomBases(fromClient, options);
        }
        
        public static void RequestAttackableBaseData(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();

            ServerSend.GiveBaseToAttackData(fromClient, id);
        }

        #endregion

        /*
        public static void RequestPreloadUpdate(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();

            ServerSend.GivePreloadUpdate(fromClient, baseID);
        }
        */

        #region Unit

        public static void AddUnit(int fromClient, Packet packet)
        {
            string name = packet.ReadString();
            int baseID = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                provider.BaseDataService.AddUnit(baseID, name);
                provider.SaveChanges();
            }
        }

        public static void RemoveUnit(int fromClient, Packet packet)
        {
            string name = packet.ReadString();
            int baseID = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                provider.BaseDataService.RemoveUnit(baseID, name);
                provider.SaveChanges();
            }
        }

        #endregion

        #region Resources

        public static void AddResources(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();
            int gold = packet.ReadInt();
            int elixir = packet.ReadInt();

            if (gold < 0 || elixir < 0)
            {
                Console.WriteLine("Gold and elixir must be non negative!");
                return;
            }

            if(gold + elixir == 0)
            {
                Console.WriteLine("Gold or elixir must be positive!");
                return;
            }

            using (var provider = new ServiceProvider())
            {
                provider.BaseDataService.AddResources(baseID, gold, elixir);
                provider.SaveChanges();
            }

            ServerSend.UpdateResources(fromClient, baseID);
        }

        public static void SubtractResources(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();
            int gold = packet.ReadInt();
            int elixir = packet.ReadInt();

            if (gold < 0 || elixir < 0)
            {
                Console.WriteLine("Gold and elixir must be non negative!");
                return;
            }

            if (gold + elixir == 0)
            {
                Console.WriteLine("Gold or elixir must be positive!");
                return;
            }

            using (var provider = new ServiceProvider())
            {
                provider.BaseDataService.SubtractResources(baseID, gold, elixir);
                provider.SaveChanges();
            }

            ServerSend.UpdateResources(fromClient, baseID);
        }

        public static void AddGems(int fromClient, Packet packet)
        {
            string username = packet.ReadString();
            int gems = packet.ReadInt();

            if (gems <= 0)
            {
                Console.WriteLine("Gems must be positive!");
                return;
            }

            using (var provider = new ServiceProvider())
            {
                var player = provider.PlayerDataService.Get(username);
                player.Gems += gems;
                provider.SaveChanges();
            }

            ServerSend.UpdateGems(fromClient, username);
        }

        public static void SubtractGems(int fromClient, Packet packet)
        {
            string username = packet.ReadString();
            int gems = packet.ReadInt();

            if(gems <= 0)
            {
                Console.WriteLine("Gems must be positive!");
                return;
            }

            using (var provider = new ServiceProvider())
            {
                var player = provider.PlayerDataService.Get(username);
                player.Gems -= gems;
                provider.SaveChanges();
            }

            ServerSend.UpdateGems(fromClient, username);
        }

        #endregion

        #region Update Stored

        public static void UpdateMineStored(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            int value = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                var mine = provider.MineService.Get(id);
                mine.Stored = value;
                provider.SaveChanges();
            }
        }

        public static void UpdateStorageStored(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            int value = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                var storage = provider.StorageService.Get(id);
                storage.Stored = value;
                provider.SaveChanges();
            }
        }

        public static void UpdateMainHallStored(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            int gold = packet.ReadInt();
            int elixir = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                var hall = provider.MainHallService.Get(id);
                hall.StoredGold = gold;
                hall.StoredElixir = elixir;
                provider.SaveChanges();
            }
        }

        #endregion

        public static void AddBuilder(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                var _base = provider.BaseDataService.Get(baseID);
                _base.Builders++;

                provider.SaveChanges();
            }
        }

        public static void LeavingBase(int fromClient, Packet packet)
        {
            int baseID = packet.ReadInt();

            using (var provider = new ServiceProvider())
            {
                var _base = provider.BaseDataService.Get(baseID);
                _base.LastVisited = DateTime.Now;
            }
        }

        public static void Disconnected(int fromClient, Packet packet)
        {
            int id = packet.ReadInt();
            DateTime lastOnline = packet.ReadDateTime();

            using (var provider = new ServiceProvider())
            {
                //to do
            }
        }
    }
}
