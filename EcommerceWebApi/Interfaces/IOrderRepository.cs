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
    /// 訂單操作介面
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// 透過查詢取得所有訂單資訊
        /// </summary>
        /// <returns></returns>
        Task<PagedList<OrderDto>> GetOrders(OrderParams orderParams);
        /// <summary>
        /// 取得使用者訂單
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="orderParams"></param>
        /// <returns></returns>
        Task<PagedList<OrderDto>> GetUserOrders(string UserId, OrderParams orderParams);
        /// <summary>
        /// 透過OrderId取得OrderMaster
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        Task<OrderMaster> GetOrderMasterById(int OrderId);
        /// <summary>
        /// 透過UserId和OrderId取得訂單
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        Task<OrderMaster> GetOrderMasterByUserIdAndOrderId(string UserId, int OrderId);
        /// <summary>
        /// 透過訂單流水號取得訂單數量
        /// </summary>
        /// <param name="orderSerialNumber"></param>
        /// <returns></returns>
        Task<int> GetOrderCountBySerialNumberLike(string orderSerialNumber);
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="orderMaster"></param>
        void AddOrder(OrderMaster orderMaster);
        /// <summary>
        /// 新增至暫存庫存
        /// </summary>
        /// <param name="tempStocks">暫存庫存資料</param>
        void AddTempStocks(List<TempStock> tempStocks);
        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="orderMaster"></param>
        void UpdateOrder(OrderMaster orderMaster);
        /// <summary>
        /// 儲存資訊
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAllAsync();
    }
}
