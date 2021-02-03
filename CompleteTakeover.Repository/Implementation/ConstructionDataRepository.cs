using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    internal class ConstructionDataRepository : Repository<ConstructionData, int>, IConstructionDataRepository
    {
        public ConstructionDataRepository(DbModel context)
        :base(context)
        {

        }
    }
}
