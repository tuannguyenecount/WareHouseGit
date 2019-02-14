using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface ISlideService
    {
        List<Slide> GetAll();

        Slide GetById(int id);

        void Add(Slide slide);

        void Update(Slide slide);

        void Delete(int id);

        void CreateTranslation(SlideTranslation slideTranslation);

        void EditTranslation(SlideTranslation slideTranslation);

        void DeleteTranslation(int SlideId, string LanguageId);

    }
}
