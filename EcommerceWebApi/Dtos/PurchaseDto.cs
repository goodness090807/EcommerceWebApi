using EcommerceWebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 進貨單資訊
    /// </summary>
    public class PurchaseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 進貨單號
        /// </summary>
        public string PurchaseNum { get; set; }
        /// <summary>
        /// 總進貨量
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// 總清點量
        /// </summary>
        public int TotalCheckAmount { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 進貨狀態
        /// </summary>
        public string PurchaseStatus { get; set; }
        /// <summary>
        /// 進貨單創建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 進貨單明細
        /// </summary>
        public List<PurchaseDetailDto> PurchaseDetails { get; set; }
    }
}
