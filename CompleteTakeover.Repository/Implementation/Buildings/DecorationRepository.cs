using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Repository.Buildings
{
    internal class DecorationRepository : BuildingInstanceRepository<DecorationData>, IDecorationRepository
    {
        public DecorationRepository(DbModel context)
           : base(context)
        {

        }
    }
}
