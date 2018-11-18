using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    public class LocationController : Controller
    {
        private IProvinceService _proviceService;
        private IDistrictService _districtService;
        private IWardService _wardService;

        public LocationController(IProvinceService proviceService, IDistrictService districtService, IWardService wardService)
        {
            _proviceService = proviceService;
            _districtService = districtService;
            _wardService = wardService;
        }

        public PartialViewResult _GetDistrictByProvince(int ProvinceId)
        {
            ViewBag.DistrictId = new SelectList(_districtService.GetByProvince(ProvinceId), "Id", "Name");
            return PartialView();
        }

        public PartialViewResult _GetWardByDistrict(int DistrictId)
        {
            ViewBag.WardId = new SelectList(_wardService.GetByDistrict(DistrictId), "Id", "Name");
            return PartialView();
        }
    }
}