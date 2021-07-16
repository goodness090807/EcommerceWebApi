using EcommerceWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    /// <summary>
    /// 購物車操作
    /// </summary>
    public interface IShoppingCartRepository
    {
        /// <summary>
        /// 新增商品至購物車
        /// </summary>
        /// <param name="userShoppingCart"></param>
        void AddToShoppingCart(UserShoppingCart userShoppingCart);
        /// <summary>
        /// 取得使用者的購物車資料
        /// </summary>
        /// <param name="AppUserId">使用者Id</param>
        /// <returns></returns>
        Task<List<UserShoppingCart>> GetUserShoppings(string AppUserId);
        /// <summary>
        /// 取得單一一筆購物車的資訊
        /// </summary>
        /// <param name="AppUserId"></param>
        /// <param name="ProductDetailId"></param>
        /// <returns></returns>
        Task<UserShoppingCart> GetUserShoppingById(string AppUserId, int ProductDetailId);
        /// <summary>
        /// 更新購物車資訊
        /// </summary>
        /// <param name="userShoppingCart"></param>
        void Update(UserShoppingCart userShoppingCart);
        /// <summary>
        /// 刪除購物車資訊
        /// </summary>
        /// <param name="userShoppingCart"></param>
        void Delete(UserShoppingCart userShoppingCart);
        /// <summary>
        /// 儲存資訊
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAllAsync();
    }
}
