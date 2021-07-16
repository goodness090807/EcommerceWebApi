using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Extensions;
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
    /// 訂單功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="productStockRepository"></param>
        /// <param name="mapper"></param>
        public OrderController(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository
            , IProductStockRepository productStockRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// 取得所有訂單資訊
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PagedList<OrderDto>>> GetOrders(OrderParams orderParams)
        {
            return await _orderRepository.GetOrders(orderParams);
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="orderCreationDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> AddOrder(OrderCreationDto orderCreationDto)
        {
            var userId = User.GetUserId();
            var user = await _userRepository.GetUserByIdAsync(userId);

            var orderMaster = _mapper.Map<OrderMaster>(orderCreationDto);
            orderMaster.AppUser = user;
            #region 設定訂單流水號
            var orderSerialNumber = DateTime.Now.ToString("yyyyMMddhhmmss");

            var SameSerialNoCount = await _orderRepository.GetOrderBySerialNoLike(orderSerialNumber);

            orderMaster.OrderSerialNumber = orderSerialNumber + SameSerialNoCount;
            #endregion

            #region 訂單明細和暫存庫存加量
            orderMaster.OrderDetails = new List<OrderDetail>();
            List<TempStock> tempStocks = new List<TempStock>();
            foreach (var productCreationDetail in orderCreationDto.productDetails)
            {
                var productDetail = await _productRepository.GetProductDetailByIdAsync(productCreationDetail.ProductDetailId);

                if(productDetail.ProductStock.StockAmount - productDetail.ProductStock.OrderAmount
                    - productCreationDetail.Quantity < 0)
                {
                    return BadRequest($"{productDetail.Product.Name}/{productDetail.Color}/{productDetail.Size} 商品庫存不足");
                }

                OrderDetail orderDetail = new OrderDetail
                {
                    ProductName = productDetail.Product.Name,
                    Color = productDetail.Color,
                    Size = productDetail.Size,
                    Price = productDetail.OriginalPrice * productCreationDetail.Quantity,
                    Quantity = productCreationDetail.Quantity,
                    Product = productDetail.Product
                };
                // 新增至暫存庫存
                tempStocks.Add(new TempStock 
                { 
                    ProductDetailId = productCreationDetail.ProductDetailId,
                    OrderQuantity = productCreationDetail.Quantity
                });

                orderMaster.OrderDetails.Add(orderDetail);
            }
            #endregion

            #region 總金額計算
            orderMaster.OrderTotal = orderMaster.OrderDetails.Sum(x => x.Price);
            orderMaster.SubTotal = orderMaster.OrderTotal - orderMaster.Discount;
            #endregion

            _orderRepository.AddOrder(orderMaster);
            _orderRepository.AddTempStock(tempStocks);

            if (await _orderRepository.SaveAllAsync()) return Ok();

            return BadRequest("新增訂單發生錯誤");
        }
    }
}
