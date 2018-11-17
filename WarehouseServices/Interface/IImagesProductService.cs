using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Entities;

namespace Warehouse.Services.Interface
{
    public interface IImagesProductService
    {
        void AddImage(ImagesProduct imagesProduct);
        void DeleteImage(int Id);
    }
}
