using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    public class Product
    {
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
        /// 商品描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 商品原價
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 商品圖片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 商品創建時間
        /// </summary>
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 商品規格明細
        /// </summary>
        public ICollection<ProductDetail> ProductDetails { get; set; }
        /// <summary>
        /// 訂單明細
        /// </summary>
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
