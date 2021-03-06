using EcommerceWebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    /// <summary>
    /// 進貨主檔
    /// </summary>
    public class PurchaseMaster
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
        /// 備註
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 進貨狀態
        /// </summary>
        public PurchaseStatus PurchaseStatus { get; set; } = PurchaseStatus.UnAcceptance;
        /// <summary>
        /// 進貨單創建日期
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 進貨單變更日期
        /// </summary>
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 進貨單明細
        /// </summary>
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
