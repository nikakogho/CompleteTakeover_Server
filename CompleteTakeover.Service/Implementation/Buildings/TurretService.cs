using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class TurretService : BuildingInstanceService<TurretData, ITurretRepository>, ITurretService
    {
        public override ITurretRepository Repository => _unitOfWork.TurretRepository;

        public TurretService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
