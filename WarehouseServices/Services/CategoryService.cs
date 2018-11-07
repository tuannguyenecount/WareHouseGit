using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using Warehouse.Common;

namespace Warehouse.Services.Services
{
    public class CategoryService : ICategoryService
    {
        #region Private property
        private ICategoryDal _categoryDal;
        #endregion

        #region Constructor
        public CategoryService(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        #endregion

        #region Public logic method

        /// <summary>
        ///  Get All Product Categories 
        /// </summary>
        /// <returns></returns>
        public List<Category> GetAll()
        {
            return _categoryDal.GetList();
        }

        /// <summary>
        ///  Get Category By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        public Category GetById(int Id)
        {
            return _categoryDal.Get(c=>c.Id == Id);
        }

        /// <summary>
        ///  Get Category By Alias_SEO
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Category GetByAlias(string alias)
        {
            return _categoryDal.Get(c => c.Alias_SEO == alias);
        }

        public List<Category> Sorting(List<Category> categories, ENUM.SORT_TYPE sortType = ENUM.SORT_TYPE.Ascending)
        {
            return _categoryDal.SortList(categories, ENUM.SORT_TYPE.Ascending);
        }

        public IQueryable<Category> GetAllQueryable()
        {
            return _categoryDal.GetQueryable();
        }

        #endregion
    }
}
