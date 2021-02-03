using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;

namespace CompleteTakeover.Repository
{
    internal class AttackReportRepository : Repository<AttackReport, int>, IAttackReportRepository
    {
        public AttackReportRepository(DbModel context)
        :base(context)
        {

        }
    }
}
