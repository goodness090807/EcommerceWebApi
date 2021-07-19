using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    /// <summary>
    /// 商品操作的介面
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// 取得所有商品資訊
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetProductsAsync();
        /// <summary>
        /// 透過查詢參數取得可查詢的商品資訊
        /// </summary>
        /// <param name="productParams"></param>
        /// <returns></returns>
        Task<IQueryable<Product>> GetQueryableProductsAsync(ProductParams productParams);
        /// <summary>
        /// 透過查詢參數取得符合條件的商品資訊
        /// </summary>
        /// <returns></returns>
        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);
        /// <summary>
        /// 透過商品總號取得商品資料
        /// </summary>
        /// <param name="ProductGroupNum">商品總號</param>
        /// <returns></returns>
        Task<Product> GetProductByProductGroupNum(string ProductGroupNum);
        /// <summary>
        /// 透過商品Id取得商品資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Product> GetProductByIdAsync(int Id);
        /// <summary>
        /// 取得顯示上架的商品
        /// </summary>
        /// <returns></returns>
        Task<PagedList<ShowProductDto>> GetShowProducts(ProductParams productParams);
        /// <summary>
        /// 透過Id取得顯示上架的商品
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        Task<Product> GetShowProductWithDetailById(int ProductId);
        /// <summary>
        /// 透過商品Id取得符合的商品規格列表
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        Task<List<ProductDetail>> GetShowProductDetailByProductId(int ProductId);
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="product"></param>
        void AddProduct(Product product);
        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="product"></param>
        void UpdateProduct(Product product);
        /// <summary>
        /// 透過商品Id取得所有規格的資訊
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        Task<List<ProductDetail>> GetProductDetailsByProductIdAsync(int ProductId);
        /// <summary>
        /// 透過商品規格Id取得商品規格資料
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ProductDetail> GetProductDetailByIdAsync(int Id);
        /// <summary>
        /// 透過顏色和尺寸取得該商品規格資料
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="Color"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        Task<ProductDetail> GetProductDetailByColorAndSize(int ProductId, string Color, string Size);
        /// <summary>
        /// 更新商品規格資料
        /// </summary>
        /// <param name="productDetail">商品規格資料</param>
        void UpdateProductDetail(ProductDetail productDetail);
        /// <summary>
        /// 儲存資訊
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAllAsync();
    }
}
