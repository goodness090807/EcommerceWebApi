using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 進貨明細
    /// </summary>
    public class PurchaseDetailCreationDto
    {
        /// <summary>
        /// 商品明細Id
        /// </summary>
        public int ProductDetailId { get; set; }
        /// <summary>
        /// 進貨數量
        /// </summary>
        public int Amount { get; set; }
    }
}
