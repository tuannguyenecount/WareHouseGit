﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using Warehouse.Common;
using System.Web;

namespace Warehouse.Services.Services
{
    public class CategoryService : ICategoryService
    {
        #region Private property
        private ICategoryDal _categoryDal;
        private ICategoryTranslationDal _categoryTranslationDal;
        #endregion

        #region Constructor
        public CategoryService(ICategoryDal categoryDal, ICategoryTranslationDal categoryTranslationDal)
        {
            _categoryDal = categoryDal;
            _categoryTranslationDal = categoryTranslationDal;
        }
        #endregion

        #region Public logic method

        /// <summary>
        ///  Get All Product Categories 
        /// </summary>
        /// <returns></returns>
        public List<Category> GetAll()
        {
            return _categoryDal.GetList(c => c.Deleted == false && (c.Category2 != null ? (c.Category2.Deleted == false) : true )  );
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
            string languageId = HttpContext.Current.Request.Cookies["lang"].Value;
            if(languageId == "vi")
                return _categoryDal.GetSingle(c => c.Alias_SEO == alias);
            else
                return this.GetById(_categoryTranslationDal.GetSingle(c => c.Alias_SEO == alias).CategoryId);
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
            return _categoryDal.GetFirst(c => c.Name == Name) != null || _categoryTranslationDal.GetFirst(c => c.Name == Name) != null;
        }

        public bool CheckExistAlias(string Alias)
        {
            return _categoryDal.GetFirst(c => c.Alias_SEO == Alias) != null || _categoryTranslationDal.GetFirst(c => c.Alias_SEO == Alias) != null;
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

        public void CreateTranslation(CategoryTranslation categoryTranslation)
        {
            try
            {
                _categoryDal.CreateTranslation(categoryTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EditTranslation(CategoryTranslation categoryTranslation)
        {
            try
            {
                _categoryDal.EditTranslation(categoryTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteTranslation(int CategoryId, string LanguageId)
        {
            try
            {
                _categoryDal.DeleteTranslation(CategoryId, LanguageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CountByAlias(string alias)
        {
            return _categoryDal.Count(c => c.Alias_SEO == alias) + _categoryTranslationDal.Count(c => c.Alias_SEO == alias);
        }

        public int CountByName(string Name)
        {
            return _categoryDal.Count(c => c.Name == Name) + _categoryTranslationDal.Count(c => c.Name == Name);
        }


        #endregion
    }
}
