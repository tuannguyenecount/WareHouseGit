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
    public class FavoriteProductService : IFavoriteProductService
    {
        readonly IFavoriteProductDal _iFavoriteProductDal;

        public FavoriteProductService(IFavoriteProductDal iFavoriteProductDal)
        {
            _iFavoriteProductDal = iFavoriteProductDal;
        }

        public List<FavoriteProduct> GetAll(string userId)
        {
            return _iFavoriteProductDal.GetList(f=>f.AspNetUserId == userId);
        }

        public void Add(FavoriteProduct favoriteProduct)
        {
            _iFavoriteProductDal.Add(favoriteProduct);
        }

        public void Update(FavoriteProduct favoriteProduct)
        {
            _iFavoriteProductDal.Update(favoriteProduct);
        }

        public void Delete(FavoriteProduct favoriteProduct)
        {
            _iFavoriteProductDal.Delete(favoriteProduct);
        }

        public FavoriteProduct Get(string UserId, int ProductId)
        {
            return _iFavoriteProductDal.GetSingle(f => f.AspNetUserId == UserId && f.ProductId == ProductId);
        }
    }
}
