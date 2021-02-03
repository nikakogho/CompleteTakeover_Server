using CompleteTakeover.Domain.Buildings;
using CompleteTakeover.Repository;
using CompleteTakeover.Repository.Buildings;

namespace CompleteTakeover.Service.Buildings
{
    internal class TrainingZoneService : BuildingInstanceService<TrainingZoneData, ITrainingZoneRepository>, ITrainingZoneService
    {
        public override ITrainingZoneRepository Repository => _unitOfWork.TrainingZoneRepository;

        public TrainingZoneService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
        {

        }
    }
}
