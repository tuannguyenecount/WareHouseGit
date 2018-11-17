using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Common
{
    public class Format
    {
        public static string FormatCurrencyVND(int? price)
        {
            return price != null ? price.Value.ToString("#,##0").Replace(',', '.') + " VNĐ" : "";
        }
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.Day.ToString() + ", tháng " 
                        + dateTime.Month.ToString() + " " 
                        + dateTime.Year.ToString() + ", " 
                        + dateTime.Hour.ToString() + ":" + dateTime.Minute.ToString();
        }
    }
}
