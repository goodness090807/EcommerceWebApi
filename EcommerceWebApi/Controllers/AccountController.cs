using AutoMapper;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceWebApi.Controllers
{
    /// <summary>
    /// 使用者功能
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        /// <param name="tokenService"></param>
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<TokenDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName)) return BadRequest("已存在相同帳號");

            var user = _mapper.Map<AppUser>(registerDto);
            // 建立帳號
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);


            return await _tokenService.CreateToken(user);
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
        {
            var user =  _userManager.Users.SingleOrDefault(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("該帳號不存在");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("帳號密碼錯誤");

            return await _tokenService.CreateToken(user);
        }

        /// <summary>
        /// 更新Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("renewtoken")]
        public async Task<ActionResult<TokenDto>> ReNewToken()
        {
            var user = new AppUser
            {
                UserName = User.Identity.Name,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };

            return await _tokenService.CreateToken(user);
        }
    }
}
