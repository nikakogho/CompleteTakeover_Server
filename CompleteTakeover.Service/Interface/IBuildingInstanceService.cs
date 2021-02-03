using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    public interface IBuildingInstanceService<TData, TRepository> : IService<TData, int, TRepository>
        where TData : BuildingInstanceData where TRepository : IBuildingInstanceRepository<TData>
    {

    }
}
