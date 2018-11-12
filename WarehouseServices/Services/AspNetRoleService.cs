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
    public class AspNetRoleService : IAspNetRoleService
    {
        readonly IAspNetRoleDal _aspNetRoleDal;

        public AspNetRoleService(IAspNetRoleDal aspNetRoleDal)
        {
            _aspNetRoleDal = aspNetRoleDal;
        }

        public List<AspNetRole> GetAll()
        {
            return _aspNetRoleDal.GetList();
        }

        public AspNetRole GetById(string Id)
        {
            return _aspNetRoleDal.GetSingle(r => r.Id == Id);
        }
    }
}
