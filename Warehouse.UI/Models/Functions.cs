using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Warehouse.Models
{
    public class Functions
    {
        readonly static List<string> ImageExtensions = new List<string> { ".JPG", ".PNG", ".JPEG" };

        private Functions()
        {

        }
        public static string GetAllErrorsPage(ModelStateDictionary ModelState)
        {
            StringBuilder strb = new StringBuilder("<ul>");
            foreach (string key in ModelState.Keys)
            {
                ModelState[key].Errors.ToList().ForEach(m => strb.Append("<li>" + m.ErrorMessage + "</li>"));
            }
            strb.Append("</ul>");
            return strb.ToString();
        }
        public static void SaveFileFromBase64(string fileName, string base64String)
        {
            System.IO.File.WriteAllBytes(fileName, Convert.FromBase64String(base64String));
        }
        public static string GetTimePast(DateTime time)
        {
            return Math.Floor((DateTime.Now - time).TotalHours) == 0 ? Math.Ceiling((DateTime.Now - time).TotalMinutes).ToString() + " phút" : Math.Floor((DateTime.Now - time).TotalHours) > 24 ? Math.Floor((DateTime.Now - time).TotalDays).ToString() + " ngày" : Math.Floor((DateTime.Now - time).TotalHours).ToString() + " giờ";
        }
        public static void UpLoadImage(string fileName, HttpPostedFileBase file, ModelStateDictionary ModelState, string ErrorKey)
        {
            string extend = Path.GetExtension(file.FileName);
            if (ImageExtensions.Contains(extend.ToUpper()) == false)
            {
                ModelState.AddModelError(ErrorKey, "File có đuôi mở rộng không cho phép!");
            }
            else if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxContentLength"].ToString()) == false)
            {
                int MaxContentLength = int.Parse(ConfigurationManager.AppSettings["MaxContentLength"].ToString());
                if (file.ContentLength > MaxContentLength)
                {
                    object thongbao = "Độ lớn của hình chỉ được tối đa " + (MaxContentLength / 1048576).ToString() + "MB. Bạn có thể cắt bớt hình hoặc resize để độ lớn nhỏ lại.";
                    ModelState.AddModelError(ErrorKey, thongbao.ToString());
                }
                else
                {
                    try
                    {
                        file.SaveAs(fileName);
                    }
                    catch
                    {
                        ModelState.AddModelError(ErrorKey, "Xảy ra lỗi khi lưu hình!");
                    }
                }
            }
           
        }
    }
}