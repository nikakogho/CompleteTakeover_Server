using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompleteTakeover.Service
{
    using Domain;
    using Repository;

    internal class SquadDataService : Service<SquadData, int, ISquadDataRepository>, ISquadDataService
    {
        public override ISquadDataRepository Repository => _unitOfWork.SquadDataRepository;

        public SquadDataService(IUnitOfWork unitOfWork)
        :base(unitOfWork)
        {

        }
    }
}
