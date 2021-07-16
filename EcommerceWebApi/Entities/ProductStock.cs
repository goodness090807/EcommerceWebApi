using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    /// <summary>
    /// 商品庫存
    /// </summary>
    public class ProductStock
    {
        /// <summary>
        /// 商品庫存Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 庫存量
        /// </summary>
        public int StockAmount { get; set; }
        /// <summary>
        /// 訂單佔量
        /// </summary>
        public int OrderAmount { get; set; }

        /// <summary>
        /// 商品規格
        /// </summary>
        public ProductDetail ProductDetail { get; set; }
        /// <summary>
        /// 商品規格Id
        /// </summary>
        public int ProductDetailId { get; set; }
    }
}
