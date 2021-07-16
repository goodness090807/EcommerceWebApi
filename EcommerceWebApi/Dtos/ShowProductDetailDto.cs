using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    public class ShowProductDetailDto : ShowProductDto
    {
        /// <summary>
        /// 商品顏色列表
        /// </summary>
        public List<string> Colors { get; set; }
        /// <summary>
        /// 商品尺寸列表
        /// </summary>
        public List<string> Sizes { get; set; }
    }
}
