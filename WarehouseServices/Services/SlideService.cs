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

        
    }
}
