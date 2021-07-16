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
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductStock> GetProductStockByProductDetailIdAsync(int ProductDetailId)
        {
            return await _context.ProductStocks.FirstOrDefaultAsync(x => x.ProductDetailId == ProductDetailId);
        }

        public void UpdateProductStock(ProductStock productDetail)
        {
            _context.Entry(productDetail).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
