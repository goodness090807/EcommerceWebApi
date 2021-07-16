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

        public async Task<int> GetOrderBySerialNoLike(string orderSerialNumber)
        {
            return await _context.OrderMasters.Where(x => EF.Functions.Like(x.OrderSerialNumber, $"{orderSerialNumber}%")).CountAsync();
        }

        public void AddOrder(OrderMaster orderMaster)
        {
            _context.OrderMasters.Add(orderMaster);
        }

        public void AddTempStock(List<TempStock> tempStocks)
        {
            _context.TempStocks.AddRange(tempStocks);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
