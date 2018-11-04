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
            return (price ?? 0).ToString("#,##0").Replace(',', '.') + " VNĐ";
        }

    }
}
