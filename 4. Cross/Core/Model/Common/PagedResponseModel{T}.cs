using System.Collections.Generic;

namespace Core.Model.Common
{
    public class PagedResponseModel<T>
    {
        public int Total { get; set; }
        public List<T> Items { get; set; }
    }
}