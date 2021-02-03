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

    public interface IBaseDataService : IService<BaseData, int, IBaseDataRepository>
    {
        void AddUnit(int id, string name);
        void RemoveUnit(int id, string name);
        bool IsDestroyed(int id);
        MainHallData GetMainHall(int id);
        int GetTotalStoredGold(int id);
        int GetTotalStoredElixir(int id);
        void AddResources(int id, int gold, int elixir);
        void SubtractResources(int id, int gold, int elixir);
    }
}
