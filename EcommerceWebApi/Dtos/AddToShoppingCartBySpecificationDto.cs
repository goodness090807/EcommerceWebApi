using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 新增至購物車的規格參數
    /// </summary>
    public class AddToShoppingCartBySpecificationDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 商品顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 商品尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
    }
}
