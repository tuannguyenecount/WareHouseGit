using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
using Warehouse.Common;
using System.Globalization;

namespace Warehouse.Services.Services
{
    public class ProductService : IProductService 
    {
        private IProductDal _productDal;

        public bool HttpContext { get; private set; }

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
            Product product = GetById(productId);
            if (product != null)
                _productDal.Delete(product);
        }

        public List<Product> GetRelatedProducts(int? categoryId, int productMainId)
        {
            return _productDal.GetList(p => p.Display == true && p.CategoryId == categoryId && p.Id != productMainId);
        }

        public List<Product> GetNewProducts()
        {
            return _productDal.GetList(p=>p.Display == true).OrderByDescending(p=>p.Id).Take(8).ToList();
        }

        Func<DateTime, int> weekProjector = d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                 d,
                                 CalendarWeekRule.FirstFourDayWeek,
                                 DayOfWeek.Sunday);

        public List<Product> GetHotProductsInWeek()
        {
            return _productDal.GetList(p => p.Display == true).OrderByDescending(p => p.OrderDetails.Count()).ToList();
        }

        public List<Product> GetSaleProducts()
        {
            return _productDal.GetList(p => p.Display == true && p.PriceNew != null).OrderByDescending(p => p.Id).ToList();
        }

        public int CountDisplay()
        {
            return _productDal.Count(p=>p.Display == true);
        }

        public int CountHide()
        {
            return _productDal.Count(p => p.Display == false);
        }


        public List<Product> Search(string keyword)
        {
            int countWord = keyword.ToUpper().Split(' ').Count();
            return _productDal.GetList(p=>p.Display == true).Where(p => p.Name.ToUpper().Split(' ').Join(keyword.ToUpper().Split(' ').AsEnumerable(), o=>o, i=>i, (i,o) => new { inner = i, outer = o }).Count() == countWord).ToList();
        }

        public List<Product> GetByUser(string UserName)
        {
            return _productDal.GetList(p => p.UserCreated == UserName).ToList();
        }

        public bool CheckUniqueName(string Name)
        {
            return _productDal.GetFirst(p => p.Name == Name) == null;
        }

        public bool CheckUniqueAlias(string Alias)
        {
            return _productDal.GetFirst(p => p.Alias_SEO == Alias) == null;
        }

        public void CreateTranslation(ProductTranslation productTranslation)
        {
            try
            {
                _productDal.CreateTranslation(productTranslation);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EditTranslation(ProductTranslation productTranslation)
        {
            try
            {
                _productDal.EditTranslation(productTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
