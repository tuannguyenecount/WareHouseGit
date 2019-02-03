using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Warehouse.Models
{
    public class Functions
    {
        readonly static List<string> ImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"].ToString().Split('|').ToList();

        private Functions()
        {

        }
        /// <summary>
        /// Get All Errors In Page
        /// </summary>
        /// <param name="ModelState"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  Save File Image From String Base64
        /// </summary>
        /// <param name="fileName">path save image</param>
        /// <param name="base64String">String Base64</param>
        public static void SaveFileFromBase64(string path, string base64String)
        {
            System.IO.File.WriteAllBytes(path, Convert.FromBase64String(base64String));
        }

        /// <summary>
        ///  Get Time String 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimePast(DateTime time)
        {
            return Math.Floor((DateTime.Now - time).TotalHours) == 0 ? Math.Ceiling((DateTime.Now - time).TotalMinutes).ToString() + " phút" : Math.Floor((DateTime.Now - time).TotalHours) > 24 ? Math.Floor((DateTime.Now - time).TotalDays).ToString() + " ngày" : Math.Floor((DateTime.Now - time).TotalHours).ToString() + " giờ";
        }

        /// <summary>
        /// Upload image to server
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="file"></param>
        /// <param name="ModelState"></param>
        /// <param name="ErrorKey"></param>
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

        /// <summary>
        /// Check Valid Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        const string UniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
        const string KoDauChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

        public static string UnicodeToKoDau(string s)
        {
            string retVal = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                int pos = UniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            return retVal.ToLower();
        }

        public static string UnicodeToKoDauAndGach(string s)
        {
            const string strChar = "abcdefghijklmnopqrstxyzuvxw0123456789- ";
            s = UnicodeToKoDau(s.ToLower().Trim());
            string sReturn = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (strChar.IndexOf(s[i]) > -1)
                {
                    if (s[i] != ' ')
                        sReturn += s[i];
                    else if (i > 0 && s[i - 1] != ' ')
                        sReturn += "-";
                }
            }
            return sReturn.Replace("--", "-").ToLower();

            //s = UnicodeToKoDau(s.Trim().ToLower());
            //var regex = new Regex("[^a-z0-9\\-]");
            //s = regex.Replace(s, "-");
            //regex = new Regex("[\\-]{2,}");

            //return regex.Replace(s, "-").Trim('-');
        }
    }
}