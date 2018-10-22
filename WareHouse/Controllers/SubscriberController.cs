using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WareHouse.Models;
using System.Threading.Tasks;
namespace WareHouse.Controllers
{
    public class SubscriberController : Controller
    {
        hotellte_warehouseEntities db = new hotellte_warehouseEntities();
        #region front-end
        // GET: NhanTinQuaEmail
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Add(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return "Bạn chưa nhập email!";
            }
            if(ThuVien.IsValidEmail(email) == false)
            {
                return "Địa chỉ email không hợp lệ!";
            }
            try
            {
                if(db.Subscribers.FirstOrDefault(m=>m.Email == email) != null)
                {
                    return "Địa chỉ email này đã đăng ký nhận tin trước đó.";
                }
                Subscriber model = new Subscriber() { Email = email, DateSubscriber = DateTime.Today };
                db.Subscribers.Add(model);
                db.SaveChangesAsync();
                return "Đăng ký nhận tin tức qua email " + email + " thành công. Chúc bạn 1 ngày tốt lành.";
            }
            catch
            {
                return "Xảy ra lỗi ở phía server. Vui lòng thử lại sau!";
            }
        }
        #endregion
    }
}