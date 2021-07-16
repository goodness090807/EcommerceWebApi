using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 商品規格資料
    /// </summary>
    public class ProductDetailDto
    {
        /// <summary>
        /// 商品規格Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商品編號
        /// </summary>
        public string ProductNum { get; set; }
        /// <summary>
        /// 國際條碼
        /// </summary>
        public string InternationalNum { get; set; }
        /// <summary>
        /// 商品原價
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 商品顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 商品尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 庫存量
        /// </summary>
        public int StockAmount { get; set; }
        /// <summary>
        /// 訂單佔量
        /// </summary>
        public int OrderAmount { get; set; }
        /// <summary>
        /// 可以下單的量
        /// </summary>
        public int CanOrderAmount { get; set; }
    }
}
