using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface ISubscriberService
    {

        List<Subscriber> GetAll();

        Subscriber GetByEmail(string email);

        void Add(Subscriber subscriber);

        void Update(Subscriber subscriber);

        void Delete(string email);

    }
}
