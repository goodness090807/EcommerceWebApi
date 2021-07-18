using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Enums
{
    /// <summary>
    /// 訂單狀態
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 待確認接收訂單
        /// </summary>
        UnConfirm,
        /// <summary>
        /// 待出貨
        /// </summary>
        UnShip,
        /// <summary>
        /// 已出貨
        /// </summary>
        Shipped
    }
}
