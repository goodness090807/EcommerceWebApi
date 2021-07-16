using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Extensions;
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
    /// 購物車操作
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="shoppingCartRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="mapper"></param>
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IUserRepository userRepository
            , IProductRepository productRepository, IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 取得使用者的購物車資訊
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<UserShoppingCartDto>>> Get()
        {
            var userId = User.GetUserId();

            var shoppingCarts = await _shoppingCartRepository.GetUserShoppings(userId);

            return _mapper.Map<List<UserShoppingCartDto>>(shoppingCarts);
        }

        /// <summary>
        /// 將商品新增至購物車
        /// </summary>
        /// <param name="addToShoppingCartDto">商品規格Id和數量</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToShoppingCart(AddToShoppingCartDto addToShoppingCartDto)
        {
            var userId = User.GetUserId();

            var userShoppingCart = await _shoppingCartRepository.GetUserShoppingById(userId, addToShoppingCartDto.ProductDetailId);

            if(userShoppingCart == null)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                var productDetail = await _productRepository.GetProductDetailByIdAsync(addToShoppingCartDto.ProductDetailId);

                var newUserShoppingCart = new UserShoppingCart
                {
                    AppUser = user,
                    ProductDetail = productDetail,
                    Quantity = addToShoppingCartDto.Quantity
                };

                _shoppingCartRepository.AddToShoppingCart(newUserShoppingCart);
            }
            else
            {
                userShoppingCart.Quantity += addToShoppingCartDto.Quantity;
                _shoppingCartRepository.Update(userShoppingCart);
            }

            if (await _shoppingCartRepository.SaveAllAsync()) return Ok(_mapper.Map<UserShoppingCartDto>(userShoppingCart));

            return BadRequest("加入至購物車發生錯誤");
        }

        /// <summary>
        /// 將商品新增至購物車
        /// </summary>
        /// <param name="dto">商品Id、規格參數和數量</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddBySpecification")]
        public async Task<ActionResult<UserShoppingCartDto>> AddToShoppingCartBySpecification(AddToShoppingCartBySpecificationDto dto)
        {
            var userId = User.GetUserId();
            var productDetail = await _productRepository.GetProductDetailByColorAndSize(dto.Color, dto.Size);

            var userShoppingCart = await _shoppingCartRepository.GetUserShoppingById(userId, productDetail.Id);

            if (userShoppingCart == null)
            {
                var user = await _userRepository.GetUserByIdAsync(userId);

                var newUserShoppingCart = new UserShoppingCart
                {
                    AppUser = user,
                    ProductDetail = productDetail,
                    Quantity = dto.Quantity
                };

                _shoppingCartRepository.AddToShoppingCart(newUserShoppingCart);
            }
            else
            {
                userShoppingCart.Quantity += dto.Quantity;
                _shoppingCartRepository.Update(userShoppingCart);
            }

            if (await _shoppingCartRepository.SaveAllAsync()) return Ok(_mapper.Map<UserShoppingCartDto>(userShoppingCart));

            return BadRequest("加入至購物車發生錯誤");
        }

        /// <summary>
        /// 更新購物車商品數量
        /// </summary>
        /// <param name="ProductDetailId">商品規格Id</param>
        /// <param name="Quantity">更新的數量</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{ProductDetailId}")]
        public async Task<ActionResult> UpdateShoppingCartQuantity(int ProductDetailId, int Quantity)
        {
            var userId = User.GetUserId();

            var userShoppingCart = await _shoppingCartRepository.GetUserShoppingById(userId, ProductDetailId);

            if (userShoppingCart == null) return BadRequest("此商品已經從購物車移除");

            userShoppingCart.Quantity += Quantity;

            // 大於0就更新，小於等於0就刪除
            if(userShoppingCart.Quantity > 0)
                _shoppingCartRepository.Update(userShoppingCart);
            else
                _shoppingCartRepository.Delete(userShoppingCart);

            if (await _shoppingCartRepository.SaveAllAsync()) return NoContent();

            return BadRequest("更新購物車商品數量發生錯誤");
        }

        /// <summary>
        /// 從購物車移除商品
        /// </summary>
        /// <param name="ProductDetailId">商品規格Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{ProductDetailId}")]
        public async Task<ActionResult> DeleteFromShoppingCart(int ProductDetailId)
        {
            var userId = User.GetUserId();

            var userShoppingCart = await _shoppingCartRepository.GetUserShoppingById(userId, ProductDetailId);

            if (userShoppingCart == null) return BadRequest("此商品已經從購物車移除");

            _shoppingCartRepository.Delete(userShoppingCart);

            if (await _shoppingCartRepository.SaveAllAsync()) return Ok(_mapper.Map<UserShoppingCartDto>(userShoppingCart));

            return BadRequest("移除購物車商品發生錯誤");
        }

        /// <summary>
        /// 透過cache取得購物車資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet("getbycache")]
        public ActionResult GetByCache()
        {
            return Ok();
        }
    }
}
