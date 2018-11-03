using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IProductService
    {

        List<Product> GetAll();

        List<Product> GetByCategory(int categoryId, bool includeProductsHidden = false);

        Product GetById(int productId);

        Product GetByAlias(string alias);

        List<Product> GetNewProducts();

        IQueryable<Product> Sorting(IQueryable<Product> products, string sortName, ENUM.SORT_TYPE sortType);

        void Add(Product product);

        void Update(Product product);

        void Delete(int productId);

        List<Product> GetRelatedProducts(int? categoryId, int productMainId);

    }
}
