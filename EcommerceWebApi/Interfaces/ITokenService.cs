using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(AppUser appUser);
    }
}
