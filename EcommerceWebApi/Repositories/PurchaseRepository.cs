using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceWebApi.Data;
using EcommerceWebApi.Dtos;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Helpers;
using EcommerceWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PurchaseRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddPurchase(PurchaseMaster purchaseMaster)
        {
            _context.PurchaseMasters.Add(purchaseMaster);
        }

        public async Task<PagedList<PurchaseDto>> GetPurchases(PurchaseParams purchaseParams)
        {
            var query = _context.PurchaseMasters.Include(x => x.PurchaseDetails)
                .ThenInclude(x => x.ProductDetail)
                .ThenInclude(x => x.Product).AsQueryable();

            query = query.Where(x => purchaseParams.PurchaseNum == null || x.PurchaseNum == purchaseParams.PurchaseNum);

            return await PagedList<PurchaseDto>.CreateAsync(query.ProjectTo<PurchaseDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                   purchaseParams.PageNumber, purchaseParams.PageSize);
        }

        public async Task<PurchaseMaster> GetPurchaseMasterById(int PurchaseId)
        {
            return await _context.PurchaseMasters.Include(x => x.PurchaseDetails)
                .ThenInclude(x => x.ProductDetail)
                .ThenInclude(x => x.Product).Where(x => x.Id == PurchaseId).FirstOrDefaultAsync();
        }

        public async Task<List<PurchaseDetail>> GetPurchaseDetailsByPurchaseMasterId(int PurchaseMasterId)
        {
            return await _context.PurchaseDetails.Include(x => x.ProductDetail)
                .ThenInclude(x => x.Product).ToListAsync();
        }

        public void UpdatePurchaseMaster(PurchaseMaster purchaseMaster)
        {
            _context.Entry(purchaseMaster).State = EntityState.Modified;
        }

        public void UpdatePurchaseDetail(PurchaseDetail purchaseDetail)
        {
            _context.Entry(purchaseDetail).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
