using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IProvinceService
    {

        List<Province> GetAll();

        Province GetById(int id);

        void Add(Province province);

        void Update(Province province);

        void Delete(int id);

    }
}
