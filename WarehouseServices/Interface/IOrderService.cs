using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IOrderService
    {

        List<Order> GetAll();

        Order GetById(int OrderId);

        void Add(Order Order);

        void Update(Order Order);

        void Delete(int OrderId);

        int CountAll();

        int CountOrderWaitConfirm();

        List<Order> GetHistory(string userId);
    }
}
