using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Repository.Buildings
{
    internal class MainHallRepository : BuildingInstanceRepository<MainHallData>, IMainHallRepository
    {
        public MainHallRepository(DbModel context)
        : base(context)
        {

        }
    }
}
