using DbUpTrial.Entities;
using DbUpTrial.QueryResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DbUpTrial.Respositories.Abstracts
{
    public interface ICouponRepository
    {
        Task<(IEnumerable<GetAllCouponsResult>, int)> GetAllCouponsAsync(IDbConnection conn, string productName = null, int? pageSize = null, int? pageNumber = null, IDbTransaction tran = null);

        Task<GetAllCouponsResult> GetCouponAsync(IDbConnection conn, int id, IDbTransaction tran = null);

        Task InsertCouponAsync(Coupon coupon, IDbConnection conn, IDbTransaction tran = null);
    }
}
