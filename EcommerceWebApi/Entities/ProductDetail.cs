using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    public class ProductDetail
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
        /// 總商品編號
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 商品物件
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// 商品庫存物件
        /// </summary>
        public ProductStock ProductStock { get; set; }

        public ICollection<UserShoppingCart> UserShoppingCarts { get; set; }
    }
}
