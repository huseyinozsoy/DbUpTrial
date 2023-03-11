using DbUpTrial.Requests;
using DbUpTrial.Responses;
using System.Threading.Tasks;

namespace DbUpTrial.Services.Abstracts
{
    public interface ICouponService
    {
        Task<GetAllDiscountsReponse> GetAllDiscountsAsync(GetAllDiscountsRequest request);

        Task<GetDiscountReponse> GetDiscountAsync(GetDiscountRequest request);

        Task InsertCouponAsync(InsertCouponRequest request);
    }
}
