using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 訂單下單格式
    /// </summary>
    public class OrderCreationDto
    {
        /// <summary>
        /// 下訂的商品規格Id
        /// </summary>
        public List<OrderDetailCreactionDto> productDetails { get; set; }
        /// <summary>
        /// 訂單折扣
        /// </summary>
        //public int Discount { get; set; }
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
