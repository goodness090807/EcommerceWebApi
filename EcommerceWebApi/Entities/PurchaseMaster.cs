using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Entities
{
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
        public string MyProperty { get; set; }
    }
}
