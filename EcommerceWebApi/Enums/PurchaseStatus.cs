using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Enums
{
    /// <summary>
    /// 進貨狀態
    /// </summary>
    public enum PurchaseStatus
    {
        /// <summary>
        /// 待驗收
        /// </summary>
        UnAcceptance,
        /// <summary>
        /// 已驗收
        /// </summary>
        Acceptanced,
        /// <summary>
        /// 已入庫
        /// </summary>
        InStock
    }
}
