using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var Coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon where ProductName = @productName",
                    new { productName = productName });

            if(Coupon == null) {
                return new Coupon { ProductName ="No Product", Description ="No Discount Desc", Amount = 0 };
            }
            return Coupon;
        }
        
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                ("Insert into Coupon(ProductName,Description,Amount) values(@productname,@description,@amount)", 
                    new { productname = coupon.ProductName, description= coupon.Description, amount = coupon.Amount });
            
            if(affected == 0)
                return false;

            return true;
        }

        public async  Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                ("Delete FROM Coupon where ProductName =@productName",
                    new { productName = productName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName = @productName, Description = @description, Amount = @amount WHERE Id = @id",
                    new { productname = coupon.ProductName, description = coupon.Description, amount = coupon.Amount, id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
