using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.DataAccess.EntityFramework;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using System.Data.Entity;
using Warehouse.Core.DataAccess;

namespace Warehouse.Data.Data
{
    public class AspNetUserDal: EntityRepositoryBase<AspNetUser, IdentityDbContext>, IAspNetUserDal
    {
        
    }
}
