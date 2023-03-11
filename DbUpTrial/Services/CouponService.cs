using DbUpTrial.Configs;
using DbUpTrial.Entities;
using DbUpTrial.Requests;
using DbUpTrial.Responses;
using DbUpTrial.Respositories.Abstracts;
using DbUpTrial.Services.Abstracts;
using Mapster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbUpTrial.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepoistory;
        private readonly IDatabaseProvider _databaseProvider;

        public CouponService(ICouponRepository couponRepoistory, IDatabaseProvider databaseProvider)
        {
            _couponRepoistory = couponRepoistory;
            _databaseProvider = databaseProvider;
        }

        public async Task<GetAllDiscountsReponse> GetAllDiscountsAsync(GetAllDiscountsRequest request)
        {
            var response = new GetAllDiscountsReponse();
            using (var conn = _databaseProvider.GetConnection())
            {
                var (couponsResult, count) = await _couponRepoistory.GetAllCouponsAsync(conn, request.ProductName, request.pageSize, request.pageNumber);
                response.DiscountList = couponsResult.Adapt<List<DiscountsModel>>();
                response.Paginate(count, request.pageNumber, request.pageSize);
            }

            return response;
        }

        public async Task<GetDiscountReponse> GetDiscountAsync(GetDiscountRequest request)
        {
            var response = new GetDiscountReponse();
            using (var conn = _databaseProvider.GetConnection())
            {
                var couponsResult = await _couponRepoistory.GetCouponAsync(conn, request.Id);
                response.Discount = couponsResult.Adapt<DiscountsModel>();
            }

            return response;
        }

        public async Task InsertCouponAsync(InsertCouponRequest request)
        {
            using (var conn = _databaseProvider.GetConnection())
            {
                await _couponRepoistory.InsertCouponAsync(request.Adapt<Coupon>(), conn);
            }
        }
    }
}