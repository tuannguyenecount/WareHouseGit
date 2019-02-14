using System;
using System.Collections.Generic;
using Warehouse.Entities;
using Warehouse.Services.Interface;
using Warehouse.Data.Interface;
using System.Linq;
namespace Warehouse.Services.Services
{
    public class SlideService : ISlideService
    {
        private ISlideDal _slideDal;

        public SlideService(ISlideDal slideDal)
        {
            _slideDal = slideDal;
        }

        public List<Slide> GetAll()
        {
            return _slideDal.GetList();
        }

        public Slide GetById(int id)
        {
            return _slideDal.GetSingle(p => p.Id == id);
        }

        public void Update(Slide slide)
        {
            _slideDal.Update(slide);
        }

        public void Add(Slide slide)
        {
            _slideDal.Add(slide);
        }

        public void Delete(int Id)
        {
            Slide slide = GetById(Id);
            if (slide != null)
                _slideDal.Delete(slide);
        }

        public void CreateTranslation(SlideTranslation slideTranslation)
        {
            try
            {
                _slideDal.CreateTranslation(slideTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EditTranslation(SlideTranslation slideTranslation)
        {
            try
            {
                _slideDal.EditTranslation(slideTranslation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteTranslation(int SlideId, string LanguageId)
        {
            try
            {
                _slideDal.DeleteTranslation(SlideId, LanguageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
