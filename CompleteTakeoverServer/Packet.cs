using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using CompleteTakeover.Domain;
using CompleteTakeover.Service;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeoverServer
{
    public enum ServerPackets
    {
        welcomeFromServer = 1, registerConfirm, loadPlayer, buildingAdded,
        //,processAttack, processDefense,
        giveFewRandomBases, giveAttackableBaseData,
        updateResources, updateGems,
        giveUpdatedPlayerData
        //,givePreloadUpdate
    }
    public enum ClientPackets
    {
        welcomeReceived = 1, registerCheck, loadPlayerRequest, addBuilding, upgradeBuilding,
        buildingFixed,
        reportAttack, reportDefense, defenderProcessedCombat,
        requestFewRandomBases, requestAttackableBaseData,
        addUnit, removeUnit, addResources, subtractResources, addGems, subtractGems,
        moveBuilding, updateMineStored, updateStorageValue,
        leavingBase, disconnecting, addBuilder,
        requestPlayerDataUpdate
        //,requestPreloadUpdate
    }

    public class Packet : IDisposable
    {
        List<byte> buffer;
        byte[] readableBuffer;
        int readPos;

        public Packet()
        {
            buffer = new List<byte>();
            readPos = 0;
        }

        public Packet(int id)
        :this()
        {
            Write(id);
        }

        public Packet(byte[] data)
        :this()
        {
            SetBytes(data);
        }

        #region Functions

        public void SetBytes(byte[] data)
        {
            Write(data);
            readableBuffer = buffer.ToArray();
        }

        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        public void InsertInt(int value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(value));
        }

        public byte[] ToArray()
        {
            return readableBuffer = buffer.ToArray();
        }

        public int Length()
        {
            return buffer.Count;
        }

        public int UnreadLength()
        {
            return Length() - readPos;
        }

        public void Reset(bool should = true)
        {
            if (should)
            {
                buffer.Clear();
                readableBuffer = null;
                readPos = 0;
            }
            else readPos = -4;
        }

        #endregion

        #region Write Data

        #region Conventional

        public void Write(byte value)
        {
            buffer.Add(value);
        }

        public void Write(byte[] values)
        {
            buffer.AddRange(values);
        }

        public void Write(short value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(int value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(long value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(float value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(bool value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            Write(value == null);
            if (value == null) return;
            Write(value.Length);
            var bytes = Encoding.ASCII.GetBytes(value);
            Write(bytes);
        }

        #endregion

        #region Special

        public void Write(KeyValuePair<int, int> value)
        {
            Write(value.Key);
            Write(value.Value);
        }

        public void Write(DateTime value)
        {
            Write(value.Year);
            Write(value.Month);
            Write(value.Day);
            Write(value.Hour);
            Write(value.Minute);
            Write(value.Second);
        }

        public void Write(Vector3 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }

        public void Write(ConstructionData value)
        {
            Write(value.ID);
            //Write(value.Base.ID); //for checking

            Write(value.Hours);
            Write(value.Minutes);
            Write(value.Seconds);
            Write(value.TileX);
            Write(value.TileY);

            bool isUpgrade = value.Upgrading != null;
            Write(isUpgrade);

            if (isUpgrade) Write(value.Upgrading.ID);
            else Write(value.BuildingDataName);
        }

        public void Write(BuildingInstanceData value)
        {
            bool empty = value == null;
            Write(empty);
            if (empty) return;
            Write(value.ID);
            //Write(value.Base.ID); //for checking
            Write(value.DataName);

            Write(value.Level);

            Write(value.TileX);
            Write(value.TileY);
            Write(value.Destroyed);

            Write(value.BuildingType);
            switch (value.BuildingType)
            {
                case "Army Holder":
                    break;

                case "Bunker":
                    var bunker = value as BunkerData;
                    Write(bunker.StoredUnits.Count);
                    foreach (var unit in bunker.StoredUnits) Write(unit);
                    break;

                case "Lab":
                    var lab = value as LabData;
                    Write(lab.WorkingOnUnit);
                    Write(lab.HoursLeft);
                    Write(lab.MinutesLeft);
                    Write(lab.SecondsLeft);
                    break;

                case "Main Hall":
                    var hall = value as MainHallData;
                    Write(hall.StoredGold);
                    Write(hall.StoredElixir);
                    break;

                case "Mine":
                    var mine = value as MineData;
                    Write(mine.Stored);
                    break;

                case "Storage":
                    var storage = value as StorageData;
                    Write(storage.Stored);
                    break;

                case "Training Zone":
                    var zone = value as TrainingZoneData;
                    Write(zone.CurrentUnitName);
                    Write(zone.HoursLeft);
                    Write(zone.MinutesLeft);
                    Write(zone.SecondsLeft);
                    Write(zone.Slots.Count);
                    foreach (var slot in zone.Slots) Write(slot);
                    break;

                case "Turret":
                    break;

                case "Wall":
                    break;

                case "Trap":
                    break;

                case "Decoration":
                    break;
            }
        }

        public void Write(SquadData value, bool includeID = true)
        {
            if(includeID) Write(value.ID);
            //Write(value.Base.ID); may do for checking
            Write(value.UnitName);
            Write(value.Amount);
        }

        public void Write(DefenseReport value)
        {
            Write(value.ID);
            Write(value.InvasionName);
            Write(value.LostGold);
            Write(value.LostElixir);

            Write(value.DeployedArmy.Count);
            foreach (var squad in value.DeployedArmy) Write(squad, false);
        }

        public void Write(AttackReport value)
        {
            Write(value.ID);
            Write(value.Attacker.Username);
            Write(value.AmassedGold);
            Write(value.AmassedElixir);

            Write(value.DeployedArmy.Count);
            foreach (var squad in value.DeployedArmy) Write(squad, false);
        }

        public void Write(BaseData value)
        {
            bool empty = value == null;
            Write(empty);
            if (empty) return;
            Write(value.ID);
            //Write(value.Player.Username); //for checking
            Write(value.IsHome);

            Write(value.LastVisited);

            //stored gold
            //stored elixir

            var squads = value.Squads.Where(squad => squad.InArmyOfBase == value && squad.Amount > 0);
            Write(squads.Count());
            foreach (var squad in squads) Write(squad);

            Write(value.Buildings.Count);
            foreach (var building in value.Buildings) Write(building);

            Write(value.Builders);

            var constructions = value.Constructions.Where(con => !con.Over);
            Write(constructions.Count());
            foreach (var construction in constructions) Write(construction);

            var unprocessedDefenseReports = value.AttackedByAIReports.Where(report => !report.Processed);
            Write(unprocessedDefenseReports.Count());
            foreach (var report in unprocessedDefenseReports) Write(report);

            var unprocessedAttackReports = value.AttackedByPlayerReports.Where(report => !report.ProcessedOnDefendersSide);
            Write(unprocessedAttackReports.Count());
            foreach (var report in unprocessedAttackReports) Write(report);
        }

        public void Write(PlayerData value)
        {
            bool empty = value == null;
            Write(empty);
            if (empty) return;
            Write(value.Username);
            Write(value.Password.Length);
            Write(value.Password);
            Write(value.IconPath);
            Write(value.Faction);
            //Write(value.Gold);
            //rite(value.Elixir);
            Write(value.Gems);
            Write(value.LastOnline);
            //Write(value.HomeBase); no more
            Write(value.Colonies.Count);
            foreach(var colony in value.Colonies) Write(colony);
        }

        #endregion

        #endregion

        #region Read Data

        #region Conventional

        public byte ReadByte(bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                byte value = readableBuffer[readPos];
                if (moveReadPos) readPos++;
                return value;
            }
            else throw new Exception("Could not read value of type 'byte'!");
        }

        public byte[] ReadBytes(int length, bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                byte[] value = buffer.GetRange(readPos, length).ToArray();
                if (moveReadPos) readPos += length;
                return value;
            }
            else throw new Exception("Could not read value of type 'byte[]'!");
        }

        public short ReadShort(bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                short value = BitConverter.ToInt16(readableBuffer, readPos);
                if (moveReadPos) readPos += 2; //short is 2 bytes
                return value;
            }
            else throw new Exception("Could not read value of type 'short'!");
        }

        public int ReadInt(bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                int value = BitConverter.ToInt32(readableBuffer, readPos);
                if (moveReadPos) readPos += 4; //int is 4 bytes
                return value;
            }
            else throw new Exception("Could not read value of type 'int'!");
        }

        public long ReadLong(bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                long value = BitConverter.ToInt64(readableBuffer, readPos);
                if (moveReadPos) readPos += 8;
                return value;
            }
            else throw new Exception("Could not read value of type 'long'!");
        }

        public float ReadFloat(bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                float value = BitConverter.ToSingle(readableBuffer, readPos);
                if (moveReadPos) readPos += 4;
                return value;
            }
            else throw new Exception("Could not read value of type 'float'!");
        }

        public bool ReadBool(bool moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                bool value = BitConverter.ToBoolean(readableBuffer, readPos);
                if (moveReadPos) readPos++;
                return value;
            }
            else throw new Exception("Could not read value of type 'bool'!");
        }

        public string ReadString()
        {
            try
            {
                bool empty = ReadBool();
                if (empty) return null;
                int length = ReadInt();
                string value = Encoding.ASCII.GetString(readableBuffer, readPos, length);
                if (value.Length > 0) readPos += length;
                return value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }

        #endregion

        #region Special

        public DateTime ReadDateTime()
        {
            int year = ReadInt();
            int month = ReadInt();
            int day = ReadInt();
            int hour = ReadInt();
            int minute = ReadInt();
            int second = ReadInt();
            return new DateTime(year, month, day, hour, minute, second);
        }

        public Vector3 ReadVector3(bool moveReadPos = true)
        {
            return new Vector3(ReadFloat(moveReadPos), ReadFloat(moveReadPos), ReadFloat(moveReadPos));
        }

        public BuildingInstanceData ReadBuildingInstanceData()
        {
            bool empty = ReadBool();
            if (empty) return null;
            int id = ReadInt();
            //int baseID = ReadInt(moveReadPos); // may use at some point
            string dataName = ReadString();

            int level = ReadInt();

            int tileX = ReadInt();
            int tileY = ReadInt();
            bool destroyed = ReadBool();

            string type = ReadString();

            switch (type)
            {
                case "Army Holder":
                    return new ArmyHolderData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type
                    };

                case "Bunker":
                    return new BunkerData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type,
                        StoredUnits = new List<SquadData>()
                    };

                case "Lab":
                    return new LabData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type,
                        WorkingOnUnit = ReadString(),
                        HoursLeft = ReadInt(),
                        MinutesLeft = ReadInt(),
                        SecondsLeft = ReadInt()
                    };

                case "Main Hall":
                    return new MainHallData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type,
                        StoredGold = ReadInt(),
                        StoredElixir = ReadInt(),
                        GoldCapacity = ReadInt(),
                        ElixirCapacity = ReadInt()
                    };

                case "Mine":
                    return new MineData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type,
                        Stored = ReadInt(),
                        StoresGold = ReadBool()
                    };

                case "Storage":
                    return new StorageData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type,
                        Stored = ReadInt(),
                        StoresGold = ReadBool(),
                        Capacity = ReadInt()
                    };

                case "Training Zone":
                    return new TrainingZoneData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type,
                        Slots = new List<SquadData>()
                    };

                case "Turret":
                    return new TurretData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY,
                        Destroyed = destroyed,
                        BuildingType = type
                    };

                case "Trap":
                    return new TrapData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY
                    };

                case "Wall":
                    return new WallData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY
                    };

                case "Decoration":
                    return new DecorationData()
                    {
                        ID = id,
                        DataName = dataName,
                        Level = level,
                        TileX = tileX,
                        TileY = tileY
                    };

                default: return null;
            }
        }

        #endregion

        #endregion

        bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
