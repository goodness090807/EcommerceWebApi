using EcommerceWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    /// <summary>
    /// 商品庫存操作介面
    /// </summary>
    public interface IProductStockRepository
    {
        /// <summary>
        /// 透過商品規格Id取得該商品的庫存資訊
        /// </summary>
        /// <param name="ProductDetailId"></param>
        /// <returns></returns>
        Task<ProductStock> GetProductStockByProductDetailIdAsync(int ProductDetailId);
        /// <summary>
        /// 更新庫存量
        /// </summary>
        /// <param name="productDetail"></param>
        void UpdateProductStock(ProductStock productDetail);
        /// <summary>
        /// 儲存資訊
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAllAsync();
    }
}
