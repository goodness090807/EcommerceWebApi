using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Helpers
{
    public class PurchaseParams : PaginationParams
    {
        /// <summary>
        /// 進貨單號
        /// </summary>
        public string PurchaseNum { get; set; }
    }
}
