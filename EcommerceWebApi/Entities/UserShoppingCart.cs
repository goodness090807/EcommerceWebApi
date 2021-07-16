using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    /// <summary>
    /// 購物車
    /// </summary>
    public class UserShoppingCart
    {
        public AppUser AppUser { get; set; }
        /// <summary>
        /// 使用者的Id
        /// </summary>
        public string AppUserId { get; set; }
        public ProductDetail ProductDetail { get; set; }
        /// <summary>
        /// 商品規格的Id
        /// </summary>
        public int ProductDetailId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
    }
}
