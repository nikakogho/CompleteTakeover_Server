using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Repository;

namespace CompleteTakeover.Service
{
    internal class ConstructionDataService : Service<ConstructionData, int, IConstructionDataRepository>, IConstructionDataService
    {
        public override IConstructionDataRepository Repository => _unitOfWork.ConstructionDataRepository;

        public ConstructionDataService(IUnitOfWork unitOfWork)
        :base(unitOfWork)
        {

        }
    }
}
