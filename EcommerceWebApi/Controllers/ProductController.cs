using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Helpers;
using EcommerceWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Controllers
{
    /// <summary>
    /// 商品功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="fileStorageService"></param>
        /// <param name="mapper"></param>
        /// <param name="env"></param>
        public ProductController(IProductRepository productRepository, IFileStorageService fileStorageService, IMapper mapper, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _env = env;
        }

        /// <summary>
        /// 取得所有商品的資訊
        /// </summary>
        /// <returns>所有商品的資訊，包含未上架不顯示的(管理者專用)</returns>
        /// <response code="200">回傳所有商品資料</response>
        /// <response code="401">沒有相關權限</response>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProducts([FromQuery] ProductParams productParams)
        {
            return await _productRepository.GetProductsAsync(productParams);
        }

        /// <summary>
        /// 透過Id取得商品資料
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        /// <response code="200">回傳商品資料</response>
        /// <response code="401">沒有相關權限</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("{Id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProductDto>> GetProductById(int Id)
        {
            var product = await _productRepository.GetProductByIdAsync(Id);

            return _mapper.Map<ProductDto>(product);
        }

        /// <summary>
        /// 使用者可以取得的商品資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("ShowProducts")]
        public async Task<ActionResult<PagedList<ShowProductDto>>> GetShowProducts([FromQuery] ProductParams productParams)
        {
            return await _productRepository.GetShowProducts(productParams);
        }

        /// <summary>
        /// 使用者可以取得的商品明細資料
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <returns></returns>
        [HttpGet("ShowProduct/{ProductId}")]
        public async Task<ActionResult<ShowProductDetailDto>> GetShowProductWithDetailById(int ProductId)
        {
            var produt = await _productRepository.GetShowProductWithDetailById(ProductId);

            var showProductDetail = _mapper.Map<ShowProductDetailDto>(produt);

            showProductDetail.Colors = produt.ProductDetails.Select(x => x.Color).Distinct().ToList();
            showProductDetail.Sizes = produt.ProductDetails.Select(x => x.Size).Distinct().ToList();

            return showProductDetail;

        }

        /// <summary>
        /// 透過商品Id取得所有商品規格的資訊
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <returns></returns>
        [HttpGet("GetShowProductDetails/{ProductId}")]
        public async Task<ActionResult<List<ProductDetailDto>>> GetShowProductDetailsByProductId(int ProductId)
        {
            var produt = await _productRepository.GetShowProductDetailByProductId(ProductId);

            return _mapper.Map<List<ProductDetailDto>>(produt);
        }


        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="productCreation"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromForm]ProductCreationDto productCreation)
        {
            if (await CheckPgnExist(productCreation.ProductGroupNum))
                return BadRequest("該商品總號已存在，無法新增");

            var product = _mapper.Map<Product>(productCreation);

            if (productCreation.ImageFile != null)
            {
                // 將圖片路徑存進資料庫
                product.ImageUrl = await _fileStorageService.SaveFile(productCreation.ImageFile, Path.GetExtension(productCreation.ImageFile.FileName)
                    , "Images");
            }


            _productRepository.AddProduct(product);

            if(await _productRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetProductById", new { Id = product.Id}, _mapper.Map<ProductDto>(product));
            }

            return BadRequest("新增商品失敗");
        }

        /// <summary>
        /// 更新商品資料
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="productUpdateDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateProduct(int Id, [FromForm] ProductUpdateDto productUpdateDto)
        {
            var product = await _productRepository.GetProductByIdAsync(Id);
            if (product == null)
                return BadRequest("找不到相關的商品資訊");

            _mapper.Map(productUpdateDto, product);

            if (productUpdateDto.ImageFile != null)
            {
                // 將圖片路徑存進資料庫
                product.ImageUrl = await _fileStorageService.UpdateFile(productUpdateDto.ImageFile, Path.GetExtension(productUpdateDto.ImageFile.FileName)
                    , "Images", product.ImageUrl);
            }

            _productRepository.UpdateProduct(product);

            if (await _productRepository.SaveAllAsync()) return NoContent();

            return BadRequest("更新商品資料發生錯誤");
        }

        /// <summary>
        /// 新增單筆商品規格
        /// </summary>
        /// <param name="ProductId">商品Id</param>
        /// <param name="productDetailCreationDto">商品規格資料</param>
        /// <returns>Product資料</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProductDetail/{ProductId}")]
        public async Task<ActionResult> AddProductDetail(int ProductId, ProductDetailCreationDto productDetailCreationDto)
        {
            if (await CheckSpeExist(productDetailCreationDto.Color, productDetailCreationDto.Size))
                return BadRequest("已有相同的規格，無法新增!!!");

            var product = await _productRepository.GetProductByIdAsync(ProductId);

            var productDetail = _mapper.Map<ProductDetail>(productDetailCreationDto);

            // 新增商品庫存和預設庫存量
            productDetail.ProductStock = new ProductStock() { StockAmount = productDetailCreationDto.ProductStockAmount ?? 0 };
            product.ProductDetails.Add(productDetail);
            

            if (await _productRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetProductById", new { Id = product.Id }, _mapper.Map<ProductDto>(product));
            }

            return BadRequest("新增規格商品失敗");
        }

        #region 暫不使用
        ///// <summary>
        ///// 新增多筆商品規格資料
        ///// </summary>
        ///// <param name="ProductId">商品Id</param>
        ///// <param name="productDetailCreationDtos">要新增的多筆商品規格</param>
        ///// <returns></returns>
        //[HttpPost("AddProductDetails/{ProductId}")]
        //public async Task<ActionResult> AddProductDetails(int ProductId, List<ProductDetailCreationDto> productDetailCreationDtos)
        //{
        //    var product = await _productRepository.GetProductByIdAsync(ProductId);

        //    var productDetails = _mapper.Map<List<ProductDetail>>(productDetailCreationDtos);

        //    foreach(var productDetail in productDetails)
        //    {
        //        product.ProductDetails.Add(productDetail);
        //    }

        //    if (await _productRepository.SaveAllAsync())
        //    {
        //        return CreatedAtRoute("GetProductById", new { Id = product.Id }, _mapper.Map<ProductDto>(product));
        //    }

        //    return BadRequest("新增規格商品失敗");
        //}
        #endregion

        /// <summary>
        /// 單筆更新商品規格
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="productDetailCreationDto"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProductDetail/{Id}")]
        public async Task<ActionResult> UpdateProductDetail(int Id, ProductDetailCreationDto productDetailCreationDto)
        {
            var productDetail = await _productRepository.GetProductDetailByIdAsync(Id);
            if (productDetail == null)
                return BadRequest("找不到相關的商品資訊");

            _mapper.Map(productDetailCreationDto, productDetail);

            if (await _productRepository.SaveAllAsync()) return NoContent();

            return BadRequest("更新商品資料發生錯誤");
        }

        #region 暫不需要
        ///// <summary>
        ///// 取得圖片
        ///// </summary>
        ///// <param name="ImageId"></param>
        ///// <returns></returns>
        //[HttpGet("Image/{ImageId}")]
        //public ActionResult GetImage(string ImageId)
        //{
        //    // 取得靜態檔案位置
        //    var ImagePath = Path.Combine(_env.WebRootPath, "Images", ImageId);
        //    var image = System.IO.File.OpenRead(ImagePath);
        //    return File(image, "image/png");
        //}
        #endregion

        #region 驗證功能
        /// <summary>
        /// 驗證商品總號是否存在
        /// </summary>
        /// <param name="productGroupNum">商品總號</param>
        /// <returns></returns>
        private async Task<bool> CheckPgnExist(string productGroupNum)
        {
            var product = await _productRepository.GetProductByProductGroupNum(productGroupNum);
            if (product != null)
                return true;

            return false;
        }

        private async Task<bool> CheckSpeExist(string Color, string Size)
        {
            var productDetail = await _productRepository.GetProductDetailByColorAndSize(Color, Size);

            if (productDetail != null)
                return true;

            return false;
        }
        #endregion
    }
}
