using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
namespace Warehouse.Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private IOrderDetailDal _orderdetailDal;

        public OrderDetailService(IOrderDetailDal orderdetailDal)
        {
            _orderdetailDal = orderdetailDal;
        }

        public List<OrderDetail> GetAll()
        {
            return _orderdetailDal.GetList();
        }

        public OrderDetail GetById(int id)
        {
            return _orderdetailDal.GetSingle(p => p.Id == id);
        }

        public void Update(OrderDetail orderdetail)
        {
            _orderdetailDal.Update(orderdetail);
        }

        public void Add(OrderDetail orderdetail)
        {
            _orderdetailDal.Add(orderdetail);
        }

        public void Delete(int Id)
        {
            OrderDetail orderDetail = GetById(Id);
            if (orderDetail != null)
                _orderdetailDal.Delete(orderDetail);
        }
    }
}
