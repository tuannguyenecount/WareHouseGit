using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WareHouse.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Mail;
namespace WareHouse.Models
{
    public class ThuVien
    {
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static string XoaKhoangTrangThua(string s)
        {
            for (int i = 0; i < s.Length - 1; i++)
            {
                if (s[i] == ' ' && s[i + 1] == ' ')
                {
                    s = s.Remove(i, 1);
                    i--;
                }
            }

            return s.Trim();
        }
        public static string BoDauTiengViet(string str)
        {
            string[] signs = new string[] { 
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
   };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                {
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
                }
            }
            return str;
        }
        public static string[] tachTu(string s)
        {
            string[] dsTu = s.Split(' ');
            return dsTu;
        }
        
        public  static List<Product>  timProductTheoTuKhoa(string tukhoa)
        {
            hotellte_warehouseEntities db = new hotellte_warehouseEntities();
            string[] dsTu = tukhoa.ToUpper().Split(' ').Distinct().ToArray();
            List<Product> dsProduct = new List<Product>();
            foreach (string tu in dsTu)
            {
                var ds = db.Products.ToList().Where(m => tachTu(m.Name.ToUpper()).Contains(tu.ToUpper()));
                if (ds != null)
                    dsProduct.AddRange(ds.ToList());
            }
            return dsProduct.Distinct().ToList();
        }

    }
}