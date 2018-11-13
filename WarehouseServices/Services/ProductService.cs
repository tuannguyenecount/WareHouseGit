using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
using Warehouse.Common;


namespace Warehouse.Services.Services
{
    public class ProductService : IProductService 
    {
        private IProductDal _productDal;

        public ProductService(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public List<Product> GetAll()
        {
            return _productDal.GetList();
        }

        public IQueryable<Product> Sorting(IQueryable<Product> products, string sortName, ENUM.SORT_TYPE sortType = ENUM.SORT_TYPE.Ascending)
        {
            switch (sortName)
            {
                case "id":
                    {
                        products = _productDal.SortList(products, p => p.Id, sortType);
                        break;
                    }
                case "price":
                    {
                        products = _productDal.SortList(products, p => p.Price, sortType);
                        break;
                    }
                case "datecreated":
                    {
                        products = _productDal.SortList(products, p => p.DateCreated, sortType);
                        break;
                    }
                case "hot":
                    {
                        products = _productDal.SortList(products, p => p.OrderDetails.Count, sortType);
                        break;
                    }
            }
            return products.AsQueryable();
        }

        public List<Product> GetByCategory(int categoryId, bool includeProductsHidden = false)
        {
            return _productDal.GetList(p => p.CategoryId == categoryId && (includeProductsHidden == true ? true : (p.Display == true)) );
        }

        public Product GetById(int productId)
        {
            return _productDal.GetSingle(p => p.Id == productId);
        }

        public Product GetByAlias(string alias)
        {
            return _productDal.GetSingle(p => p.Alias_SEO == alias);
        }

        public void Update(Product product)
        {
            _productDal.Update(product);
        }

        public void Add(Product product)
        {
            _productDal.Add(product);
        }

        public void Delete(int productId)
        {
            _productDal.Delete(new Product { Id = productId });
        }

        public List<Product> GetRelatedProducts(int? categoryId, int productMainId)
        {
            return _productDal.GetList(p => p.Display == true && p.CategoryId == categoryId && p.Id != productMainId);
        }

        public List<Product> GetNewProducts()
        {
            return _productDal.GetList(p=>p.Display == true).OrderByDescending(p=>p.Id).Take(8).ToList();
        }

        public List<Product> GetHotProductsInWeek()
        {
            return _productDal.GetHotProductsInWeek();
        }

        public int CountAll()
        {
            return _productDal.Count();
        }

        public List<Product> Search(string keyword)
        {
            return _productDal.GetList(p => p.Name.ToUpper().Contains(keyword.ToUpper()) || true);
        }

        public List<Product> GetByUser(string UserName)
        {
            return _productDal.GetList(p => p.UserCreated == UserName).ToList();
        }
    }
}
