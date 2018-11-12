using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IAspNetUserService
    {
        List<AspNetUser> GetAll();
        AspNetUser GetById(string Id);
        AspNetUser GetByUserName(string UserName);
        void Lock(string Id, DateTime timeLock);
        List<AspNetRole> GetRolesOfUser(string Id);
    }
}
