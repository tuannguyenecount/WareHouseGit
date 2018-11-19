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
            return _categoryDal.GetSingle(c=>c.Id == Id);
        }

        /// <summary>
        ///  Get Category By Alias_SEO
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Category GetByAlias(string alias)
        {
            return _categoryDal.GetSingle(c => c.Alias_SEO == alias);
        }
        /// <summary>
        /// Count All Category
        /// </summary>
        /// <returns></returns>
        public int CountAll()
        {
            return _categoryDal.Count();
        }

        public List<Category> GetParents()
        {
            return _categoryDal.GetList(c => c.ParentId == null).OrderBy(c => c.OrderNum).ToList();
        }

        public List<Category> GetChilds(int Id)
        {
            return _categoryDal.GetList(c => c.ParentId == Id).OrderBy(c => c.OrderNum).ToList();
        }

        public bool CheckExistName(string Name)
        {
            return _categoryDal.GetFirst(c => c.Name == Name) == null;
        }

        public bool CheckExistAlias(string Alias)
        {
            return _categoryDal.GetFirst(c => c.Alias_SEO == Alias) == null;
        }

        public void Add(Category category)
        {
            _categoryDal.Add(category);
        }

        public void Update(Category category)
        {
            _categoryDal.Update(category);
        }

        public void Delete(Category category)
        {
            _categoryDal.Delete(category);
        }
        #endregion
    }
}
