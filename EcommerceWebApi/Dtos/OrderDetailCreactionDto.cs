using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 訂單下單明細
    /// </summary>
    public class OrderDetailCreactionDto
    {
        /// <summary>
        /// 商品規格Id
        /// </summary>
        public int ProductDetailId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
    }
}
