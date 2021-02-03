using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    public interface ISquadDataService : IService<SquadData, int, ISquadDataRepository>
    {

    }
}
