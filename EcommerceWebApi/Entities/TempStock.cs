using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    /// <summary>
    /// 暫存庫存
    /// </summary>
    public class TempStock
    {
        /// <summary>
        /// 庫存Key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 商品規格Id
        /// </summary>
        public int ProductDetailId { get; set; }
        /// <summary>
        /// 訂購數量
        /// </summary>
        public int OrderQuantity { get; set; }
    }
}
