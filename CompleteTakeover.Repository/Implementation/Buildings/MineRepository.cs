using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Repository.Buildings
{
    internal class MineRepository : BuildingInstanceRepository<MineData>, IMineRepository
    {
        public MineRepository(DbModel context)
            :base(context)
        {

        }
    }
}
