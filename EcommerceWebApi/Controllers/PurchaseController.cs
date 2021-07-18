using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Enums;
using EcommerceWebApi.Helpers;
using EcommerceWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Controllers
{
    /// <summary>
    /// 進貨功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductStockRepository _productStockRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="purchaseRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="productStockRepository"></param>
        /// <param name="mapper"></param>
        public PurchaseController(IPurchaseRepository purchaseRepository, IProductRepository productRepository
            , IProductStockRepository productStockRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _productRepository = productRepository;
            _productStockRepository = productStockRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// 新增進貨單
        /// </summary>
        /// <param name="purchaseCreationDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddPurchase(PurchaseCreationDto purchaseCreationDto)
        {
            var purchase = _mapper.Map<PurchaseMaster>(purchaseCreationDto);
            purchase.PurchaseNum = DateTime.Now.ToString("yyyyMMddhhmmss");

            // 驗證商品是否存在
            foreach(var purchaseDetail in purchase.PurchaseDetails)
            {
                var productDetail = await _productRepository.GetProductDetailByIdAsync(purchaseDetail.ProductDetailId);
                if (productDetail == null) return BadRequest($"查不到Id為{purchaseDetail.Id}的商品規格");
            }

            _purchaseRepository.AddPurchase(purchase);

            if (await _purchaseRepository.SaveAllAsync()) return Ok();

            return BadRequest("新增進貨單發生錯誤!!!");
        }

        /// <summary>
        /// 取得所有進貨單
        /// </summary>
        /// <param name="purchaseParams"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PagedList<PurchaseDto>>> GetPurchases([FromQuery]PurchaseParams purchaseParams)
        {
            return await _purchaseRepository.GetPurchases(purchaseParams);
        }

        /// <summary>
        /// 進貨商品驗收
        /// </summary>
        /// <param name="PurchaseId">進貨單Id</param>
        /// <param name="acceptancePurchaseDetailDtos"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin")]
        [HttpPut("AcceptancePurchases/{PurchaseId}")]
        public async Task<ActionResult> AcceptancePurchases(int PurchaseId, List<AcceptancePurchaseDetailDto> acceptancePurchaseDetailDtos)
        {
            var purchaseMaster = await _purchaseRepository.GetPurchaseMasterById(PurchaseId);

            foreach(var purchaseDetail in purchaseMaster.PurchaseDetails)
            {
                var updatePurchaseDetail = acceptancePurchaseDetailDtos.Where(x => x.Id == purchaseDetail.Id).FirstOrDefault();
                if(updatePurchaseDetail != null)
                    _mapper.Map(updatePurchaseDetail, purchaseDetail);

                _purchaseRepository.UpdatePurchaseDetail(purchaseDetail);
            }

            purchaseMaster.PurchaseStatus = PurchaseStatus.Acceptanced;
            purchaseMaster.UpdateDate = DateTime.Now;

            if (await _purchaseRepository.SaveAllAsync()) return Ok();

            return BadRequest("驗收發生錯誤");
        }
        /// <summary>
        /// 將驗收的進貨單加入庫存
        /// </summary>
        /// <param name="PurchaseId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("PurchaseInStock/{PurchaseId}")]
        public async Task<ActionResult> PurchaseInStock(int PurchaseId)
        {
            var purchaseMaster = await _purchaseRepository.GetPurchaseMasterById(PurchaseId);

            if (purchaseMaster.PurchaseStatus != PurchaseStatus.Acceptanced) return BadRequest("不是驗收狀態不能入庫");

            foreach(var purchaseDetail in purchaseMaster.PurchaseDetails)
            {
                var productStock = await _productStockRepository.GetProductStockByProductDetailIdAsync(purchaseDetail.ProductDetailId);
                productStock.StockAmount += purchaseDetail.CheckAmount;
                _productStockRepository.UpdateProductStock(productStock);
            }
            // 變更為已入庫
            purchaseMaster.PurchaseStatus = PurchaseStatus.InStock;

            _purchaseRepository.UpdatePurchaseMaster(purchaseMaster);

            if (await _purchaseRepository.SaveAllAsync()) return Ok();

            return BadRequest("進貨入庫失敗!!!");
        }
    }
}
