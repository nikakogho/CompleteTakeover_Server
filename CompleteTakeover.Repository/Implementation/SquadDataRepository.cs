using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    internal class SquadDataRepository : Repository<SquadData, int>, ISquadDataRepository
    {
        public SquadDataRepository(DbModel context)
        : base(context)
        {
            
        }
    }
}
