using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 進貨主檔
    /// </summary>
    public class PurchaseCreationDto
    {
        /// <summary>
        /// 進貨備註
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 進貨單明細
        /// </summary>
        public List<PurchaseDetailCreationDto> PurchaseDetails { get; set; }
    }
}
