using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
namespace Warehouse.Services.Services
{
    public class LanguageService : ILanguageService
    {
        private ILanguageDal _languageDal;

        public LanguageService(ILanguageDal languageDal)
        {
            _languageDal = languageDal;
        }

        public List<Language> GetAll()
        {
            return _languageDal.GetList();
        }

        public Language GetById(string Id)
        {
            return _languageDal.GetSingle(p => p.Id == Id);
        }
    }
}
