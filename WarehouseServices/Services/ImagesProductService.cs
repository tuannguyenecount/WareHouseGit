using System;
using Warehouse.Data.Interface;
using Warehouse.Entities;
using Warehouse.Services.Interface;

namespace Warehouse.Services.Services
{
    public class ImagesProductService : IImagesProductService
    {
        readonly IImagesProductDal _imagesProductDal;

        public ImagesProductService(IImagesProductDal imagesProductDal)
        {
            _imagesProductDal = imagesProductDal;
        }

        public void AddImage(ImagesProduct imagesProduct)
        {
            _imagesProductDal.Add(imagesProduct);
        }

        public void DeleteImage(int Id)
        {
            ImagesProduct imagesProduct = _imagesProductDal.GetSingle(i => i.Id == Id);
            if (imagesProduct != null)
                _imagesProductDal.Delete(imagesProduct);
        }
    }
}
