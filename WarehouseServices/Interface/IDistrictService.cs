using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IDistrictService
    {
        List<District> GetByProvince(int ProvinceId);

        List<District> GetAll();

        District GetById(int id);
    }
}
