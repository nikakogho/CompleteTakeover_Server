using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    internal class DefenseReportRepository : Repository<DefenseReport, int>, IDefenseReportRepository
    {
        public DefenseReportRepository(DbModel context)
        :base(context)
        {

        }
    }
}
