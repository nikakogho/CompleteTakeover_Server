using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompleteTakeover.Domain;
using CompleteTakeover.Domain.Buildings;

namespace CompleteTakeover.Repository.Buildings
{
    internal class StorageRepository : BuildingInstanceRepository<StorageData>, IStorageRepository
    {
        public StorageRepository(DbModel context)
        :base(context)
        {

        }
    }
}
