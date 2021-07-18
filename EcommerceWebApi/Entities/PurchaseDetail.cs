using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    /// <summary>
    /// 進貨明細
    /// </summary>
    public class PurchaseDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 進貨數量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 清點量
        /// </summary>
        public int CheckAmount { get; set; }

        /// <summary>
        /// 進貨主檔
        /// </summary>
        public PurchaseMaster PurchaseMaster { get; set; }
        /// <summary>
        /// 進貨主檔Id
        /// </summary>
        public int PurchaseMasterId { get; set; }

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
