using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;

namespace Warehouse.Data.Data
{
    public class WardDal : EntityRepositoryBase<Ward, WarehouseContext>, IWardDal
    {
    }
}
