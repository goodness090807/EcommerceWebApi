using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 進貨驗收更新資料
    /// </summary>
    public class AcceptancePurchaseDetailDto
    {
        /// <summary>
        /// 進貨明細商品Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 驗收量
        /// </summary>
        public int CheckAmount { get; set; }
    }
}
