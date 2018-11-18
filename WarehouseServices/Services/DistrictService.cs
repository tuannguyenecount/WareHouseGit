using System.Collections.Generic;
using System.Linq;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class DistrictService : IDistrictService
    {
        private IDistrictDal _districtDal;

        public DistrictService(IDistrictDal districtDal)
        {
            _districtDal = districtDal;
        }

        public List<District> GetAll()
        {
            return _districtDal.GetList().OrderBy(d => d.SortOrder).ToList();
        }

        public District GetById(int Id)
        {
            return _districtDal.GetSingle(d=>d.Id == Id);
        }

        public List<District> GetByProvince(int ProvinceId)
        {
            return _districtDal.GetList(p => p.ProvinceId == ProvinceId).OrderBy(p=>p.SortOrder).ToList();
        }
    }
}
