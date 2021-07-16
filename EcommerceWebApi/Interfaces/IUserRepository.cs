using EcommerceWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(string UserId);
    }
}
