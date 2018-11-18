using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
namespace Warehouse.Services.Services
{
    public class ProvinceService : IProvinceService
    {
        private IProvinceDal _provinceDal;

        public ProvinceService(IProvinceDal provinceDal)
        {
            _provinceDal = provinceDal;
        }

        public List<Province> GetAll()
        {
            return _provinceDal.GetList().OrderBy(p=>p.SortOrder).ToList();
        }

        public Province GetById(int id)
        {
            return _provinceDal.GetSingle(p => p.Id == id);
        }

        public void Update(Province province)
        {
            _provinceDal.Update(province);
        }

        public void Add(Province province)
        {
            _provinceDal.Add(province);
        }

        public void Delete(int id)
        {
            _provinceDal.Delete(new Province { Id = id });
        }
    }
}
