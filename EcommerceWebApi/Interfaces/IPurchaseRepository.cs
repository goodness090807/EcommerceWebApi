using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    public interface IPurchaseRepository
    {
        /// <summary>
        /// 新增進貨單
        /// </summary>
        /// <param name="purchaseMaster"></param>
        void AddPurchase(PurchaseMaster purchaseMaster);
        /// <summary>
        /// 取得所有進貨單
        /// </summary>
        /// <returns></returns>
        Task<PagedList<PurchaseDto>> GetPurchases(PurchaseParams purchaseParams);
        /// <summary>
        /// 透過Id取得進貨單
        /// </summary>
        /// <param name="PurchaseId">進貨單Id</param>
        /// <returns></returns>
        Task<PurchaseMaster> GetPurchaseMasterById(int PurchaseId);
        /// <summary>
        /// 透過進貨單主檔Id取得進貨明細
        /// </summary>
        /// <param name="PurchaseMasterId"></param>
        /// <returns></returns>
        Task<List<PurchaseDetail>> GetPurchaseDetailsByPurchaseMasterId(int PurchaseMasterId);
        /// <summary>
        /// 更新進貨主檔
        /// </summary>
        /// <param name="purchaseMaster"></param>
        void UpdatePurchaseMaster(PurchaseMaster purchaseMaster);
        /// <summary>
        /// 更新進貨明細
        /// </summary>
        void UpdatePurchaseDetail(PurchaseDetail purchaseDetail);
        /// <summary>
        /// 儲存資訊
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAllAsync();
    }
}
