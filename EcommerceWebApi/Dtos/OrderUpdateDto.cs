using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 更新訂單資訊
    /// </summary>
    public class OrderUpdateDto
    {
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
    }
}
