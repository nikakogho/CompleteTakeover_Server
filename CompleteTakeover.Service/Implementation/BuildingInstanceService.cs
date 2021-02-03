using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    internal abstract class BuildingInstanceService<TData, TRepository>
        : Service<TData, int, TRepository>, IBuildingInstanceService<TData, TRepository>
        where TData : BuildingInstanceData where TRepository : IBuildingInstanceRepository<TData>
    {
        public BuildingInstanceService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
