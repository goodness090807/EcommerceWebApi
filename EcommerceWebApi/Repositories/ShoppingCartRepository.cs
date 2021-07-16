using EcommerceWebApi.Data;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddToShoppingCart(UserShoppingCart userShoppingCart)
        {
            _context.UserShoppingCarts.Add(userShoppingCart);
        }

        public async Task<List<UserShoppingCart>> GetUserShoppings(string AppUserId)
        {
            return await _context.UserShoppingCarts.Where(x => x.AppUserId == AppUserId)
                .Include(x => x.ProductDetail).ThenInclude(x => x.Product).ToListAsync();
        }

        public async Task<UserShoppingCart> GetUserShoppingById(string AppUserId, int ProductDetailId)
        {
            return await _context.UserShoppingCarts.Where(x => x.AppUserId == AppUserId && x.ProductDetailId == ProductDetailId).FirstOrDefaultAsync();
        }
        public void Update(UserShoppingCart userShoppingCart)
        {
            _context.Entry(userShoppingCart).State = EntityState.Modified;
        }

        public void Delete(UserShoppingCart userShoppingCart)
        {
            _context.UserShoppingCarts.Remove(userShoppingCart);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
