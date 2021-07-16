using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 商品價格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderMasterId { get; set; }
        public OrderMaster OrderMaster { get; set; }

    }
}
