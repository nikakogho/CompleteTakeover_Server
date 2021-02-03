using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class TrapService : BuildingInstanceService<TrapData, ITrapRepository>, ITrapService
    {
        public override ITrapRepository Repository => _unitOfWork.TrapRepository;

        public TrapService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
