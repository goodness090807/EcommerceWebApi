using EcommerceWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceWebApi.Services
{
    public class UpdateToProductStockService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        /// <summary>
        /// 建構子
        /// </summary>
        public UpdateToProductStockService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var tempStocks = await context.TempStocks.ToListAsync();
                foreach(var tempStock in tempStocks)
                {
                    var productStock = await context.ProductStocks.Where(x => x.ProductDetailId == tempStock.ProductDetailId).FirstOrDefaultAsync();
                    if(productStock != null)
                    {
                        productStock.OrderAmount += tempStock.OrderQuantity;
                    }

                    context.TempStocks.Remove(tempStock);
                }
                await context.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
