using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;

namespace Warehouse.Data.Data
{
    public class DistrictDal : EntityRepositoryBase<District, WarehouseContext>, IDistrictDal
    {
    }

}
