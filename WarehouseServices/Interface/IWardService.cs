using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IWardService
    {
        List<Ward> GetByDistrict(int DistrictID);

        List<Ward> GetAll();

        Ward GetById(int Id);
    }
}
