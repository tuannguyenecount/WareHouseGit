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
    public class InfoShopService : IInfoShopService
    {
        readonly IInfoShopDal _infoShopDal;

        public InfoShopService(IInfoShopDal infoShopDal)
        {
            _infoShopDal = infoShopDal;
        }

        public InfoShop GetFirst()
        {
            return _infoShopDal.Get();
        }
    }
}
