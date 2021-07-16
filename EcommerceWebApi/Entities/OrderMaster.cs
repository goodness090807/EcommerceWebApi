using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
    /// <summary>
    /// 訂單主檔
    /// </summary>
    public class OrderMaster
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderSerialNumber { get; set; }
        /// <summary>
        /// 下單日期
        /// </summary>
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 訂單總計
        /// </summary>
        public decimal OrderTotal { get; set; }
        /// <summary>
        /// 訂單折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 訂單小計
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// 收件人電話
        /// </summary>
        public string ReceiverPhone { get; set; }
        /// <summary>
        /// 收件地址
        /// </summary>
        public string ReceiveAddress { get; set; }
        /// <summary>
        /// 訂單備註
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 訂單狀態
        /// </summary>
        public string OrderStatus { get; set; } = "unShip";


        /// <summary>
        /// 訂購人帳號Id
        /// </summary>
        public string AppUserId { get; set; }
        /// <summary>
        /// 訂購人帳號資訊
        /// </summary>
        public AppUser AppUser { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
