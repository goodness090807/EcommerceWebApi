using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 商品規格庫存
    /// </summary>
    public class ProductStockDetailDto
    {
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
        /// 商品顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 商品尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 庫存量
        /// </summary>
        public int StockAmount { get; set; }
    }
}
