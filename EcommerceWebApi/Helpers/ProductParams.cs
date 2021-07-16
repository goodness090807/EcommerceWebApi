using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Helpers
{
    /// <summary>
    /// 商品查詢參數
    /// </summary>
    public class ProductParams : PaginationParams
    {
        /// <summary>
        /// 顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 價格起
        /// </summary>
        public decimal? PriceBegin { get; set; }
        /// <summary>
        /// 價格迄
        /// </summary>
        public decimal? PriceEnd { get; set; }
        /// <summary>
        /// 商品名稱(關鍵字)
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品排序(預設商品建立時間遞減)
        /// 
        /// 參數說明:
        ///   
        ///   商品建立時間遞增：CreateDateTimeAsc
        ///   
        ///   商品建立時間遞減：CreateDateTimeDesc
        ///   
        ///   商品價格遞增：PriceAsc
        ///   
        ///   商品價格遞減：PriceDesc
        /// </summary>
        public string OrderBy { get; set; }
    }
}
