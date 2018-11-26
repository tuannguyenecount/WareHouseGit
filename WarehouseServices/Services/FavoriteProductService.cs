﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class FavoriteProductService : IFavoriteProductService
    {
        readonly IFavoriteProductDal _iFavoriteProductDal;

        public FavoriteProductService(IFavoriteProductDal iFavoriteProductDal)
        {
            _iFavoriteProductDal = iFavoriteProductDal;
        }

        public List<FavoriteProduct> GetAll()
        {
            return _iFavoriteProductDal.GetList();
        }
    }
}