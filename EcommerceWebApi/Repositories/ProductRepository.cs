using EcommerceWebApi.Data;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcommerceWebApi.Helpers;
using EcommerceWebApi.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace EcommerceWebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            //return await _context.Products.ToListAsync();
            return await _context.Products.Include(x => x.ProductDetails).ThenInclude(x => x.ProductStock).ToListAsync();
        }

        public async Task<IQueryable<Product>> GetQueryableProductsAsync(ProductParams productParams)
        {
            var query = _context.Products.Include(x => x.ProductDetails).ThenInclude(x => x.ProductStock).AsQueryable();
            // 查詢商品的參數
            query = ProductQuery(query, productParams);

            return query;
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams)
        {
            var query = _context.Products.Include(x => x.ProductDetails).ThenInclude(x => x.ProductStock).AsQueryable();
            // 查詢商品的參數
            query = ProductQuery(query, productParams);

            var product = query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            return await PagedList<ProductDto>.CreateAsync(product, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            return await _context.Products.Include(x => x.ProductDetails)
                .SingleOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Product> GetProductByProductGroupNum(string ProductGroupNum)
        {
            return await _context.Products.Include(x => x.ProductDetails)
                .SingleOrDefaultAsync(x => x.ProductGroupNum == ProductGroupNum);
        }

        public async Task<PagedList<ShowProductDto>> GetShowProducts(ProductParams productParams)
        {
            var query = _context.Products.Include(x => x.ProductDetails).AsQueryable();
            // 只能找出Show的資料
            query = query.Where(x => x.IsShow == true);
            query = query.Where(x => x.ProductDetails.Select(y => y.IsShow).Contains(true));
            // 查詢商品的參數
            query = ProductQuery(query, productParams);
            // AutoMapper綁定
            var product = query.ProjectTo<ShowProductDto>(_mapper.ConfigurationProvider);

            return await PagedList<ShowProductDto>.CreateAsync(product, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<Product> GetShowProductWithDetailById(int ProductId)
        {
            return await _context.Products.Include(x => x.ProductDetails)
                .Where(x => x.IsShow == true && x.ProductDetails.Select(y => y.IsShow).Contains(true))
                .FirstOrDefaultAsync(x => x.Id == ProductId);
        }

        public async Task<List<ProductDetail>> GetShowProductDetailByProductId(int ProductId)
        {
            return await _context.ProductDetails.Include(x => x.ProductStock).Where(x => x.IsShow == true).ToListAsync();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public void UpdateProductDetail(ProductDetail productDetail)
        {
            _context.Entry(productDetail).State = EntityState.Modified;
        }

        public async Task<List<ProductDetail>> GetProductDetailsByProductIdAsync(int ProductId)
        {
            return await _context.ProductDetails.Include(x => x.ProductStock).Where(x => x.ProductId == ProductId).ToListAsync();
        }

        public async Task<ProductDetail> GetProductDetailByIdAsync(int Id)
        {
            return await _context.ProductDetails
                .Include(x => x.Product)
                .Include(x => x.ProductStock)
                .SingleOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<ProductDetail> GetProductDetailByColorAndSize(int ProductId, string Color, string Size)
        {
            return await _context.ProductDetails.Include(x => x.ProductStock)
                .Where(x => x.ProductId == ProductId && x.Color == Color && x.Size == Size)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        #region 私有方法
        private IQueryable<Product> ProductQuery(IQueryable<Product> query, ProductParams productParams)
        {
            // 顏色
            query = query.Where(x => string.IsNullOrEmpty(productParams.Color) 
                        || x.ProductDetails.Select(y => y.Color).Contains(productParams.Color));
            // 尺寸
            query = query.Where(x => string.IsNullOrEmpty(productParams.Size)
                        || x.ProductDetails.Select(y => y.Size).Contains(productParams.Size));
            // 價格區間
            query = query.Where(x => productParams.PriceBegin == null
                        || x.OriginalPrice >= productParams.PriceBegin);
            query = query.Where(x => productParams.PriceEnd == null
                        || x.OriginalPrice <= productParams.PriceEnd);
            // 商品名稱
            query = query.Where(x => productParams.ProductName == null || x.Name.Contains(productParams.ProductName));
            // 排序
            switch (productParams.OrderBy)
            {
                case "CreateDateTimeAsc":
                    query = query.OrderBy(x => x.CreateDateTime);
                    break;
                case "CreateDateTimeDesc":
                    query = query.OrderByDescending(x => x.CreateDateTime);
                    break;
                case "PriceAsc":
                    query = query.OrderBy(x => x.OriginalPrice);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(x => x.OriginalPrice);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreateDateTime);
                    break;
            }


            return query;
        }
        #endregion
    }
}
