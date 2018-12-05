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
        List<FavoriteProduct> GetAll();
        void Add(FavoriteProduct favorite);
        void Update(FavoriteProduct favorite);
        void Delete(FavoriteProduct favorite);
    }
}
