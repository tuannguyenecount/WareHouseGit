using System.Collections.Generic;
using System.Linq;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class DistrictService : IDistrictService
    {
        IDistrictDal _districtDal;
        public List<District> GetByProvince(int ProvinceId)
        {
            return _districtDal.GetList(p => p.ProvinceId == ProvinceId).OrderBy(p=>p.SortOrder).ToList();
        }
    }
}
