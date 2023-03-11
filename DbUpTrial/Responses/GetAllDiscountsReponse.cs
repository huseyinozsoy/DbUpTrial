using DbUpTrial.Responses.Base;
using System.Collections.Generic;

namespace DbUpTrial.Responses
{
    public class GetAllDiscountsReponse: PagedResponseBase
    {
        public GetAllDiscountsReponse()
        {
            DiscountList = new List<DiscountsModel>();
        }
        public List<DiscountsModel> DiscountList { get; set; }
    }
}
