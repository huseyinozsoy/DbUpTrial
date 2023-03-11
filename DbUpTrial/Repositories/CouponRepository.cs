using Dapper;
using DbUpTrial.Entities;
using DbUpTrial.Extension;
using DbUpTrial.QueryResults;
using DbUpTrial.Respositories.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DbUpTrial.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        //https://csharp.hotexamples.com/examples/-/SqlBuilder/-/php-sqlbuilder-class-examples.html
        public async Task<(IEnumerable<GetAllCouponsResult>, int)> GetAllCouponsAsync(IDbConnection conn, string productName = null, int? pageSize = null, int? pageNumber = null, IDbTransaction tran = null)
        {
            var builder = new SqlBuilder();
            var query = @"SELECT /**select**/ 
                        FROM coupon 
                        /**where**/ 
                        /**orderby**/";
            var template = builder.AddTemplate(query);
            builder
                .Select("id, productname, description, amount")
                .Where("amount > @amount")
                .OrderBy("productname desc");

            if (!string.IsNullOrEmpty(productName))
                builder.Where($"productname ilike '{productName}%'");

            //var count = await conn.ExecuteScalarAsync<int>(template.GetCountQuery(), new { amount = 15 });

            var (result,count) = await conn.QueryAsync<GetAllCouponsResult>(template.RawSql, new { amount = 15 }, pageSize, pageNumber, tran);

            return (result, count.Value);

        }

        public async Task InsertCouponAsync(Coupon coupon, IDbConnection conn, IDbTransaction tran = null)
        {
            var sql = @"INSERT INTO public.coupon
                        (productname, description, amount)
                        VALUES(@productname, @description, @amount);";
            await conn.ExecuteAsync(sql, coupon, tran);
        }

        public async Task<GetAllCouponsResult> GetCouponAsync(IDbConnection conn, int id, IDbTransaction tran = null)
        {
            var builder = new SqlBuilder();
            var query = @"SELECT /**select**/ 
                        FROM coupon 
                        /**where**/";
            var template = builder.AddTemplate(query);
            builder
                .Select("id, productname, description, amount")
                .Where("id = @id");

            return await conn.QuerySingleAsync<GetAllCouponsResult>(template.RawSql, new { id }, tran);
        }
    }
}
