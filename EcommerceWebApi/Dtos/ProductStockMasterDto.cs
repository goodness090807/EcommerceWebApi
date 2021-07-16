using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 商品主表與庫存量
    /// </summary>
    public class ProductStockMasterDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商品總號
        /// </summary>
        public string ProductGroupNum { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品圖片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 庫存總量
        /// </summary>
        public int StockAmount { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsShow { get; set; }
    }
}
