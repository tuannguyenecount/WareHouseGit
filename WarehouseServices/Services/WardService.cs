using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class WardService : IWardService
    {
        IWardDal _wardDal;

        public WardService(IWardDal wardDal)
        {
            _wardDal = wardDal;
        }

        public List<Ward> GetAll()
        {
            return _wardDal.GetList().OrderBy(w => w.SortOrder).ToList();
        }

        public List<Ward> GetByDistrict(int DistrictID)
        {
            return _wardDal.GetList(w => w.DistrictID == DistrictID).OrderBy(w=>w.SortOrder).ToList();
        }

        public Ward GetById(int Id)
        {
            return _wardDal.GetSingle(w => w.Id == Id);
        }
    }
}
