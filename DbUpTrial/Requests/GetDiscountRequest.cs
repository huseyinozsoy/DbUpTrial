using DbUpTrial.Requests.Base;
using System;

namespace DbUpTrial.Requests
{
    public class GetDiscountRequest : RequestBase
    {
        public int Id { get; set; }
    }
}
