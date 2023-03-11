using DbUpTrial.Responses.Base;

namespace DbUpTrial.Responses
{
    public class GetDiscountReponse : ResponseBase
    {
        public DiscountsModel Discount { get; set; }
    }
}
