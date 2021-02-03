using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Domain.Buildings;
    using Repository;

    internal class BaseDataService : Service<BaseData, int, IBaseDataRepository>, IBaseDataService
    {
        public override IBaseDataRepository Repository => _unitOfWork.BaseDataRepository;

        public BaseDataService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }

        public void AddUnit(int baseID, string name)
        {
            var _base = Get(baseID);
            var squad = _base.Squads.Where(s => s.InArmyOfBase != null && s.UnitName == name).FirstOrDefault();
            if (squad == null)
            {
                squad = new SquadData()
                {
                    InArmyOfBase = _base,
                    UnitName = name,
                    Amount = 1
                };
                _unitOfWork.SquadDataRepository.Add(squad);
            }
            else squad.Amount++;
        }

        public void RemoveUnit(int baseID, string name)
        {
            var _base = Get(baseID);
            var squad = _base.Squads.Where(s => s.InArmyOfBase != null && s.UnitName == name).FirstOrDefault();
            if (squad == null || squad.Amount == 0)
                Console.WriteLine($"Unit {name} not found in base {baseID}!");
            else
            {
                squad.Amount--;
                //if(squad.Amount == 0) something
            }
        }

        public bool IsDestroyed(int baseID)
        {
            var _base = Get(baseID);
            foreach (var building in _base.Buildings) if (!building.Destroyed) return false;
            return true;
        }

        public MainHallData GetMainHall(int baseID)
        {
            return _unitOfWork.MainHallRepository.Find(h => h.BaseDataID == baseID).FirstOrDefault();
        }

        public int GetTotalStoredGold(int baseID)
        {
            int total = GetMainHall(baseID).StoredGold;
            foreach (var mine in _unitOfWork.MineRepository.Find(m => m.BaseDataID == baseID && m.StoresGold))
                total += mine.Stored;
            foreach (var storage in _unitOfWork.StorageRepository.Find(s => s.BaseDataID == baseID && s.StoresGold))
                total += storage.Stored;
            return total;
        }

        public int GetTotalStoredElixir(int baseID)
        {
            int total = GetMainHall(baseID).StoredElixir;
            foreach (var mine in _unitOfWork.MineRepository.Find(m => m.BaseDataID == baseID && !m.StoresGold))
                total += mine.Stored;
            foreach (var storage in _unitOfWork.StorageRepository.Find(s => s.BaseDataID == baseID && !s.StoresGold))
                total += storage.Stored;
            return total;
        }

        public void AddResources(int id, int gold, int elixir)
        {
            var hall = GetMainHall(id);

            var storages = _unitOfWork.StorageRepository.Find(s => s.BaseDataID == id && !s.Destroyed);

            var goldStorages = storages.Where(s => s.StoresGold);
            var elixirStorages = storages.Where(s => !s.StoresGold);

            int canTakeGold = hall.GoldCapacity - hall.StoredGold;
            int toTakeGold = Math.Min(gold, canTakeGold);

            int canTakeElixir = hall.ElixirCapacity - hall.StoredElixir;
            int toTakeElixir = Math.Min(elixir, canTakeGold);

            hall.StoredGold += toTakeGold;
            hall.StoredElixir += toTakeElixir;
            gold -= toTakeGold;
            elixir -= toTakeElixir;

            foreach (var storage in goldStorages)
            {
                int canTake = storage.Capacity - storage.Stored;
                int toTake = Math.Min(gold, canTake);
                storage.Stored += toTake;
                gold -= toTake;
                if (gold == 0) break;
            }

            foreach (var storage in elixirStorages)
            {
                int canTake = storage.Capacity - storage.Stored;
                int toTake = Math.Min(elixir, canTake);
                storage.Stored += toTake;
                elixir -= toTake;
                if (elixir == 0) break;
            }
        }
        
        public void SubtractResources(int id, int gold, int elixir)
        {
            var hall = GetMainHall(id);

            var goldStorages = _unitOfWork.StorageRepository.Find(s => s.BaseDataID == id && s.StoresGold && !s.Destroyed);
            var elixirStorages = _unitOfWork.StorageRepository.Find(s => s.BaseDataID == id && !s.StoresGold && !s.Destroyed);

            int canTakeGold = hall.StoredGold;
            int toTakeGold = Math.Min(gold, canTakeGold);

            int canTakeElixir = hall.StoredElixir;
            int toTakeElixir = Math.Min(elixir, canTakeGold);

            hall.StoredGold -= toTakeGold;
            hall.StoredElixir -= toTakeElixir;
            gold -= toTakeGold;
            elixir -= toTakeElixir;

            foreach (var storage in goldStorages)
            {
                int canTake = storage.Stored;
                int toTake = Math.Min(gold, canTake);
                storage.Stored -= toTake;
                gold -= toTake;
                if (gold == 0) break;
            }

            foreach (var storage in elixirStorages)
            {
                int canTake = storage.Stored;
                int toTake = Math.Min(elixir, canTake);
                storage.Stored -= toTake;
                elixir -= toTake;
                if (elixir == 0) break;
            }
        }
    }
}
