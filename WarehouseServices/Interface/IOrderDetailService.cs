using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IOrderDetailService
    {

        List<OrderDetail> GetAll();

        OrderDetail GetById(int id);

        void Add(OrderDetail orderdetail);

        void Update(OrderDetail orderdetail);

        void Delete(int orderdetailid);

    }
}
