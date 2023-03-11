namespace DbUpTrial.Requests.Base
{
    public class PagedRequestBase
    {
        const int maxPageSize = 50;

        public int pageNumber { get; set; } = 1;

        private int _pageSize { get; set; } = 10;

        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value <= 0 ? 10 : value;
            }
        }
    }
}
