using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 進貨單明細資訊
    /// </summary>
    public class PurchaseDetailDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProudctName { get; set; }
        /// <summary>
        /// 商品顏色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 商品尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 進貨數量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 清點量
        /// </summary>
        public int CheckAmount { get; set; }
    }
}
