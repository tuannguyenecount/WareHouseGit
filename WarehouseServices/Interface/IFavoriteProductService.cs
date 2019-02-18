using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IFavoriteProductService
    {
        List<FavoriteProduct> GetAll(string userId);
        void Add(FavoriteProduct favorite);
        void Update(FavoriteProduct favorite);
        FavoriteProduct Get(string UserId, int ProductId);
        void Delete(FavoriteProduct favorite);
        int Count(string UserId);
    }
}
