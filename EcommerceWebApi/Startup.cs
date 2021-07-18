using EcommerceWebApi.Data;
using EcommerceWebApi.Entities;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Repositories;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductStockRepository, ProductStockRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllers();
            // Identity�]�w
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = true;
            })
            // �o��O�ϥ�Core���ܭn�]�w�n�ޥΪ��\��
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<IdentityRole>>()
            // AddIdentity �� AddIdentityCore���n�[
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            // JWT�]�w
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ���ҵo���
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JwtSettings:Issuer"],
                        // ���Ҩ����A�q�`���ݭn
                        ValidateAudience = false,

                        // �ݭn�L���ɶ�(�ݻݨD)
                        //RequireExpirationTime = true,

                        // �@��ڭ̳��|���� Token �����Ĵ���
                        ValidateLifetime = true,
                        // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
                        ValidateIssuerSigningKey = true,
                        // ����key
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:TokenKey"]))

                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "�q�Ӻ���API", 
                    Version = "v1",
                    Contact = new OpenApiContact { Name = "���a��(Terry)", Email = "b0972834939@gmail.com" },
                    Description = "�q�Ӻ���API",
                });

                // �[�Jxml�ɮר�swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);

                #region �[�J���v��
                var openApiSecurity = new OpenApiSecurityScheme
                {
                    Description = "JWT�{�ұ��v�A�ϥΪ����b�U�ؤ���JBearer {token}�Atoken�гz�L�n�J��o",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                c.AddSecurityDefinition("oauth2", openApiSecurity);
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                #endregion
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommerceWebSite v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
