using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWebApi.Dtos;
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
    /// 庫存資訊
    /// </summary>
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStockController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductStockRepository _productStockRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="mapper"></param>
        public ProductStockController(IProductRepository productRepository, IProductStockRepository productStockRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productStockRepository = productStockRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 取得查詢後的商品與庫存資訊
        /// </summary>
        /// <param name="productParams"></param>
        /// <returns></returns>
        /// <response code="200">回傳所有商品與庫存資料</response>
        /// <response code="401">沒有相關權限</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedList<ProductStockMasterDto>>> GetProductStockMaster([FromQuery] ProductParams productParams)
        {
            var product = await _productRepository.GetQueryableProductsAsync(productParams);

            var productStock = product.ProjectTo<ProductStockMasterDto>(_mapper.ConfigurationProvider);

            return await PagedList<ProductStockMasterDto>.CreateAsync(productStock, productParams.PageNumber, productParams.PageSize);
        }

        /// <summary>
        /// 取得商品庫存規格明細
        /// </summary>
        /// <returns></returns>
        /// <response code="200">回傳商品規格的庫存資料</response>
        /// <response code="401">沒有相關權限</response>
        [HttpGet("GetProductStockDetails/{ProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ProductStockDetailDto>>> GetProductStockDetails(int ProductId)
        {
            var productDetail = await _productRepository.GetProductDetailsByProductIdAsync(ProductId);

            return _mapper.Map<List<ProductStockDetailDto>>(productDetail);
        }

        [HttpPut("UpdateProductStock/{ProductDetailId}")]
        public async Task<ActionResult> UpdateProductStock(int ProductDetailId, int Amount)
        {
            var productStock = await _productStockRepository.GetProductStockByProductDetailIdAsync(ProductDetailId);

            productStock.StockAmount += Amount;

            if (productStock.StockAmount < 0) return BadRequest("商品庫存量不足夠");

            _productStockRepository.UpdateProductStock(productStock);

            if (await _productStockRepository.SaveAllAsync()) return NoContent();

            return BadRequest("更新商品庫存發生錯誤");
        }
    }
}
