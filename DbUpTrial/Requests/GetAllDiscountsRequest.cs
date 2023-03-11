using DbUpTrial.Requests.Base;

namespace DbUpTrial.Requests
{
    public class GetAllDiscountsRequest: PagedRequestBase
    {
        public string ProductName { get; set; }
    }
}
