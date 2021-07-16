using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 加入購物車
    /// </summary>
    public class AddToShoppingCartDto
    {
        /// <summary>
        /// 商品規格編號
        /// </summary>
        public int ProductDetailId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
    }
}
