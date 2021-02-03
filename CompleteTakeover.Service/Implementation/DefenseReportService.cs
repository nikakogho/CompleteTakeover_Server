using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Repository;

namespace CompleteTakeover.Service
{
    internal class DefenseReportService : Service<DefenseReport, int, IDefenseReportRepository>, IDefenseReportService
    {
        public override IDefenseReportRepository Repository => _unitOfWork.DefenseReportRepository;

        public DefenseReportService(IUnitOfWork unitOfWork)
        :base(unitOfWork)
        {

        }
    }
}
