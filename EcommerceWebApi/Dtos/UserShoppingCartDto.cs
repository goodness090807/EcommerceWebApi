using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    public class UserShoppingCartDto
    {
        public string ProductNum { get; set; }
        public string InternationalNum { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
