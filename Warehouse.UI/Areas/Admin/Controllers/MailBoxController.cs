using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Warehouse.Models;
using PagedList.Mvc;
using PagedList;
namespace Warehouse.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class MailBoxController : Controller
    {
        private hotellte_WarehouseEntities db = new hotellte_WarehouseEntities();

        // GET: Admin/MailBox
        public async Task<ActionResult> Index(int? page)
        {
            List<MailBox> mailBoxes = await db.MailBoxes.OrderByDescending(m=>m.Id). ToListAsync();
            return View(mailBoxes.ToPagedList(page ?? 1, 10));
        }

        // POST: Admin/MailBox/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, int? page)
        {
            try
            {
                
                MailBox mailBox = await db.MailBoxes.FindAsync(id);
                db.MailBoxes.Remove(mailBox);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { page = page });
            }
            catch(Exception ex)
            {
                ViewBag.ThongBaoLoi = "Xảy ra lỗi. '" + ex.Message + "'";
                return View("Index",db.MailBoxes.OrderByDescending(m => m.Id).ToPagedList(page ?? 1, 10));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
