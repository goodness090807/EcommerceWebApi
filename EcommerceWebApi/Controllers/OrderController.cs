using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Enums;
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
        /// 建構子
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
        public async Task<ActionResult<PagedList<OrderDto>>> GetOrders([FromQuery] OrderParams orderParams)
        {
            return await _orderRepository.GetOrders(orderParams);
        }

        /// <summary>
        /// 取得使用者所擁有的訂單
        /// </summary>
        /// <param name="orderParams"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetUserOrders")]
        public async Task<ActionResult<PagedList<OrderDto>>> GetUserOrders([FromQuery] OrderParams orderParams)
        {
            var userId = User.GetUserId();

            return await _orderRepository.GetUserOrders(userId, orderParams);
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
            // 同樣訂單編號要額外計算取得流水號碼
            var SameSerialNoCount = await _orderRepository.GetOrderCountBySerialNumberLike(orderSerialNumber);
            // 加上流水號碼
            orderMaster.OrderSerialNumber = orderSerialNumber + SameSerialNoCount;
            #endregion

            #region 訂單明細和暫存庫存加量
            orderMaster.OrderDetails = new List<OrderDetail>();
            List<TempStock> tempStocks = new List<TempStock>();
            foreach (var productCreationDetail in orderCreationDto.productDetails)
            {
                var productDetail = await _productRepository.GetProductDetailByIdAsync(productCreationDetail.ProductDetailId);

                if (productDetail.ProductStock.StockAmount - productDetail.ProductStock.OrderAmount
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
            _orderRepository.AddTempStocks(tempStocks);

            if (await _orderRepository.SaveAllAsync()) return Ok();

            return BadRequest("新增訂單發生錯誤");
        }

        /// <summary>
        /// 更新訂單(只有待確認的訂單能變更)
        /// </summary>
        /// <param name="OrderId">訂單Id</param>
        /// <param name="orderUpdateDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{OrderId}")]
        public async Task<ActionResult> UpdateOrder(int OrderId, OrderUpdateDto orderUpdateDto)
        {
            var UserId = User.GetUserId();

            var order = await _orderRepository.GetOrderMasterByUserIdAndOrderId(UserId, OrderId);

            if (order == null) return BadRequest("查無該訂單");
            if (order.OrderStatus != OrderStatus.UnConfirm) return BadRequest("確認後的訂單無法修改");

            _mapper.Map(orderUpdateDto, order);
            _orderRepository.UpdateOrder(order);

            if (await _orderRepository.SaveAllAsync()) return NoContent();

            return BadRequest("變更訂單資訊發生錯誤");
        }

        /// <summary>
        /// 取消訂單
        /// </summary>
        /// <param name="OrderId">訂單Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("CancelOrder/{OrderId}")]
        public async Task<ActionResult> CancelOrder(int OrderId)
        {
            var UserId = User.GetUserId();

            var order = await _orderRepository.GetOrderMasterByUserIdAndOrderId(UserId, OrderId);
            if (order == null) return BadRequest("查無該訂單");
            if (order.OrderStatus != OrderStatus.UnConfirm) return BadRequest("已被確認的訂單無法取消");


            List<TempStock> tempStocks = new List<TempStock>();
            foreach(var orderDetail in order.OrderDetails)
            {
                var productDetail = await _productRepository.GetProductDetailByColorAndSize(orderDetail.ProductId, orderDetail.Color, orderDetail.Size);
                // 新增至暫存庫存
                tempStocks.Add(new TempStock
                {
                    ProductDetailId = productDetail.Id,
                    OrderQuantity = -orderDetail.Quantity
                });
            }

            order.OrderStatus = OrderStatus.Cancel;
            _orderRepository.UpdateOrder(order);
            _orderRepository.AddTempStocks(tempStocks);

            if (await _orderRepository.SaveAllAsync()) return NoContent();

            return BadRequest("取消訂單資訊發生錯誤");
        }

        /// <summary>
        /// 確認訂單(確認後不能修改訂單資訊)
        /// </summary>
        /// <param name="OrderId">訂單Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("ConfirmOrder/{OrderId}")]
        public async Task<ActionResult> ConfirmOrder(int OrderId)
        {
            var order = await _orderRepository.GetOrderMasterById(OrderId);
            // 確認訂單，變成待出貨
            order.OrderStatus = OrderStatus.UnShip;
            _orderRepository.UpdateOrder(order);

            if (await _orderRepository.SaveAllAsync()) return NoContent();

            return BadRequest("確認訂單發生錯誤");
        }

        /// <summary>
        /// 透過訂單Id做訂單出貨
        /// </summary>
        /// <param name="OrderIds"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("ShipOrderByOrderIds")]
        public async Task<ActionResult> ShipOrderByOrderId(List<int> OrderIds)
        {
            List<string> ErrorOrders = new List<string>();
            foreach(var orderId in OrderIds)
            {
                var orderMaster = await _orderRepository.GetOrderMasterById(orderId);

                if(orderMaster.OrderStatus != OrderStatus.UnShip)
                {
                    ErrorOrders.Add($"訂單編號：{orderMaster.OrderSerialNumber}，不為待出貨，不能做訂單出貨");
                    continue;
                }

                foreach (var orderDetail in orderMaster.OrderDetails)
                {
                    var productDetail = await _productRepository.GetProductDetailByColorAndSize(orderDetail.ProductId, orderDetail.Color, orderDetail.Size);
                    productDetail.ProductStock.StockAmount -= orderDetail.Quantity;
                    productDetail.ProductStock.OrderAmount -= orderDetail.Quantity;
                    // 更新商品資料
                    _productRepository.UpdateProductDetail(productDetail);
                }

                orderMaster.OrderStatus = OrderStatus.Shipped;
                orderMaster.UpdateDate = DateTime.Now;
                _orderRepository.UpdateOrder(orderMaster);
            }

            if (ErrorOrders.Count > 0) return BadRequest(string.Join("\r\n", ErrorOrders));

            if (await _orderRepository.SaveAllAsync()) return Ok("出貨成功");

            return BadRequest("訂單出貨發生錯誤!!!");
        }
    }
}
