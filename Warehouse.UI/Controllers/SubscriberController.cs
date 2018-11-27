using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using System.Threading.Tasks;
using Warehouse.Services.Interface;

namespace Warehouse.Controllers
{
    public class SubscriberController : Controller
    {
        private ISubscriberService _subscriberService;

        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("dang-ky-nhan-tin")]
        public ActionResult Register(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Bạn chưa nhập email!");
            }
            if(Functions.IsValidEmail(email) == false)
            {
                ModelState.AddModelError("", "Địa chỉ email không hợp lệ!");
            }
            if(_subscriberService.GetByEmail(email) != null)
            {
                ModelState.AddModelError("", "Email đã tồn tại!");
            }
            if (ModelState.IsValid)
            {
                _subscriberService.Add(new Entities.Subscriber() { Email = email, DateSubscriber = DateTime.Now });
                return View("SuccessSubscriber");
            }
            return View("ErrorSubscriber");
        }
    }
}