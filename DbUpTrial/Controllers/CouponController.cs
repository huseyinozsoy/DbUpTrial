using DbUpTrial.Requests;
using DbUpTrial.Responses;
using DbUpTrial.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DbUpTrial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CouponController : ControllerBase
    {

        private readonly ILogger<CouponController> _logger;

        private readonly ICouponService _couponService;

        public CouponController(
            ILogger<CouponController> logger, 
            ICouponService couponService
            )
        {
            _logger = logger;
            _couponService = couponService;
        }

        [HttpPost("all")]
        public async Task<GetAllDiscountsReponse> GetAllDiscounts([FromBody]GetAllDiscountsRequest request)
        {
            return await _couponService.GetAllDiscountsAsync(request);
        }

        [HttpPost]
        public async Task InsertDiscount([FromBody]InsertCouponRequest request)
        {
            await _couponService.InsertCouponAsync(request);
        }

        [HttpGet()]
        public async Task<GetDiscountReponse> GetDiscount([FromQuery] GetDiscountRequest request)
        {
            return await _couponService.GetDiscountAsync(request);
        }
    }
}
