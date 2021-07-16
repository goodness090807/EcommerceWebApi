using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Helpers
{
    /// <summary>
    /// 分頁參數
    /// </summary>
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        /// <summary>
        /// 頁數
        /// </summary>
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        /// <summary>
        /// 顯示筆數
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
