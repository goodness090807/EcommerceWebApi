using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    public class ProductCreationDto
    {
        /// <summary>
        /// 商品總號
        /// </summary>
        [Required]
        public string ProductGroupNum { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// 商品原價
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 商品檔案
        /// </summary>
        public IFormFile ImageFile { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        [Required]
        public bool IsShow { get; set; }
    }
}
