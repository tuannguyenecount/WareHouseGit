using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class AspNetUserService : IAspNetUserService
    {
        readonly IAspNetUserDal _aspNetUserDal;

        public AspNetUserService(IAspNetUserDal aspNetUserDal)
        {
            _aspNetUserDal = aspNetUserDal;
        }

        public List<AspNetUser> GetAll()
        {
            return _aspNetUserDal.GetList();
        }

        public AspNetUser GetById(string Id)
        {
            return _aspNetUserDal.GetSingle(u => u.Id == Id);
        }

        public AspNetUser GetByUserName(string UserName)
        {
            return _aspNetUserDal.GetSingle(u => u.UserName == UserName);
        }

        public List<AspNetRole> GetRolesOfUser(string Id)
        {
            AspNetUser aspNetUser = GetById(Id);
            return aspNetUser.AspNetRoles.ToList();
        }

        public void Lock(string Id, DateTime timeLock)
        {
            AspNetUser aspNetUser = GetById(Id);
            if(aspNetUser != null)
            {
                aspNetUser.LockoutEndDateUtc = timeLock;
                _aspNetUserDal.Update(aspNetUser);
            }
        }
    }
}
