using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Repository;

namespace CompleteTakeover.Service
{
    internal class AttackReportService : Service<AttackReport, int, IAttackReportRepository>, IAttackReportService
    {
        public override IAttackReportRepository Repository => _unitOfWork.AttackReportRepository;

        public AttackReportService(IUnitOfWork unitOfWork)
        :base(unitOfWork)
        {
            
        }
    }
}
