using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Repository
{
    internal class BaseDataRepository : Repository<BaseData, int>, IBaseDataRepository
    {
        const int midX = 30, midY = 40;

        public BaseDataRepository(DbModel context)
        :base(context)
        {

        }

        public BaseData CreateNewHomeBase(string faction)
        {
            var data = new BaseData()
            {
                IsHome = true,
                Builders = 1,
                LastVisited = DateTime.Now
            };

            var hall = new MainHallData()
            {
                DataName = faction + " Main Hall",
                TileX = midX,
                TileY = midY,
                StoredGold = 1000,
                StoredElixir = 1000,
                Level = 1,
                GoldCapacity = 9000,
                ElixirCapacity = 9000
            };

            _context.MainHalls.Add(hall);

            data.Buildings = new List<BuildingInstanceData> { hall };

            Add(data);

            return data;
        }
    }
}
