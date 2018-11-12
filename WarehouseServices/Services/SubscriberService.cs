using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
namespace Warehouse.Services.Services
{
    public class SubscriberService : ISubscriberService
    {
        private ISubscriberDal _subscriberDal;

        public SubscriberService(ISubscriberDal subscriberDal)
        {
            _subscriberDal = subscriberDal;
        }

        public List<Subscriber> GetAll()
        {
            return _subscriberDal.GetList();
        }

        public Subscriber GetByEmail(string email)
        {
            return _subscriberDal.GetFirst(p => p.Email == email);
        }

        public void Update(Subscriber email)
        {
            _subscriberDal.Update(email);
        }

        public void Add(Subscriber email)
        {
            _subscriberDal.Add(email);
        }

        public void Delete(string email)
        {
            _subscriberDal.Delete(new Subscriber { Email = email });
        }
    }
}
