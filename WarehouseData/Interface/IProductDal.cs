﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Common;
using Warehouse.Core.DataAccess;
using Warehouse.Entities;

namespace Warehouse.Data.Interface
{
    public interface IProductDal : IEntityRepository<Product>
    {
        IQueryable<Product> SortList(IQueryable<Product> entities, Expression<Func<Product, dynamic>> sorting = null, ENUM.SORT_TYPE sortType = ENUM.SORT_TYPE.Descending);
        void CreateTranslation(ProductTranslation productTranslation);
        void EditTranslation(ProductTranslation productTranslation);
        void DeleteTranslation(int ProductId, string LanguageId);
    }
}
