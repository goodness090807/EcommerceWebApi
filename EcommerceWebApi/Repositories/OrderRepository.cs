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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedList<OrderDto>> GetOrders(OrderParams orderParams)
        {
            var query = _context.OrderMasters.AsQueryable();

            return await PagedList<OrderDto>.CreateAsync(query.ProjectTo<OrderDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                   orderParams.PageNumber, orderParams.PageSize);
        }

        public async Task<PagedList<OrderDto>> GetUserOrders(string UserId, OrderParams orderParams)
        {
            var query = _context.OrderMasters.AsQueryable();
            query = query.Where(x => x.AppUserId == UserId);

            return await PagedList<OrderDto>.CreateAsync(query.ProjectTo<OrderDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                   orderParams.PageNumber, orderParams.PageSize);
        }

        public async Task<OrderMaster> GetOrderMasterById(int OrderId)
        {
            return await _context.OrderMasters.Include(x => x.OrderDetails).Where(x => x.Id == OrderId).FirstOrDefaultAsync();
        }

        public async Task<OrderMaster> GetOrderMasterByUserIdAndOrderId(string UserId, int OrderId)
        {
            return await _context.OrderMasters.Include(x => x.OrderDetails).Where(x => x.AppUserId == UserId && x.Id == OrderId).FirstOrDefaultAsync();
        }

        public async Task<int> GetOrderCountBySerialNumberLike(string orderSerialNumber)
        {
            return await _context.OrderMasters.Where(x => EF.Functions.Like(x.OrderSerialNumber, $"{orderSerialNumber}%")).CountAsync();
        }

        public void AddOrder(OrderMaster orderMaster)
        {
            _context.OrderMasters.Add(orderMaster);
        }

        public void AddTempStocks(List<TempStock> tempStocks)
        {
            _context.TempStocks.AddRange(tempStocks);
        }

        public void UpdateOrder(OrderMaster orderMaster)
        {
            _context.Entry(orderMaster).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
