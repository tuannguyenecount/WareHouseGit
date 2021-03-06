﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Common;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IProductService
    {

        List<Product> GetAll();

        List<Product> GetByCategory(int categoryId, bool includeProductsHidden = false);

        Product GetById(int productId);

        Product GetByAlias(string alias);

        List<Product> GetByUser(string UserId);

        List<Product> GetNewProducts();

        List<Product> GetHotProductsInWeek();

        List<Product> GetSaleProducts();

        List<Product> Search(string keyword);

        IQueryable<Product> Sorting(IQueryable<Product> products, string sortName, ENUM.SORT_TYPE sortType);

        void Add(Product product);

        void Update(Product product);

        void Delete(int productId);

        List<Product> GetRelatedProducts(int? categoryId, int productMainId);

        bool CheckUniqueName(string Name);

        bool CheckUniqueAlias(string Alias);

        int CountDisplay();

        int CountHide();

        int CountByAlias(string alias);

        int CountByName(string Name);

        bool CheckExistName(string Name);

        bool CheckExistAlias(string Alias);

        void CreateTranslation(ProductTranslation productTranslation);

        void EditTranslation(ProductTranslation productTranslation);

        void DeleteTranslation(int ProductId, string LanguageId);
    }
}
