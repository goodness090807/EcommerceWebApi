using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // 使用者
            CreateMap<RegisterDto, AppUser>();
            // 商品
            CreateMap<ProductCreationDto, Product>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ShowProductDto>();
            CreateMap<Product, ShowProductDetailDto>();
            // 商品明細
            CreateMap<ProductDetailCreationDto, ProductDetail>();
            CreateMap<ProductDetail, ProductDetailDto>()
                .ForMember(dest => dest.StockAmount, opt => opt.MapFrom(src => src.ProductStock.StockAmount))
                .ForMember(dest => dest.OrderAmount, opt => opt.MapFrom(src => src.ProductStock.OrderAmount))
                .ForMember(dest => dest.StockAmount, opt => opt.MapFrom(src => (src.ProductStock.StockAmount - src.ProductStock.OrderAmount) < 0 ? 0 : (src.ProductStock.StockAmount - src.ProductStock.OrderAmount)));
            // 購物車
            CreateMap<UserShoppingCart, UserShoppingCartDto>()
                .ForMember(dest => dest.ProductNum, opt => opt.MapFrom(src => src.ProductDetail.ProductNum))
                .ForMember(dest => dest.InternationalNum, opt => opt.MapFrom(src => src.ProductDetail.InternationalNum))
                .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.ProductDetail.OriginalPrice))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.ProductDetail.Color))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.ProductDetail.Size))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductDetail.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProductDetail.Product.ImageUrl));
            CreateMap<AddToShoppingCartDto, UserShoppingCartDto>();
            // 商品庫存
            CreateMap<Product, ProductStockMasterDto>()
                .ForMember(dest => dest.StockAmount, opt => opt.MapFrom(src => src.ProductDetails.Sum(x => x.ProductStock.StockAmount)));
            CreateMap<ProductDetail, ProductStockDetailDto>()
                .ForMember(dest => dest.StockAmount, opt => opt.MapFrom(src => src.ProductStock.StockAmount));
            // 訂單主檔
            CreateMap<OrderCreationDto, OrderMaster>();
            CreateMap<OrderMaster, OrderDto>();

        }
    }
}
