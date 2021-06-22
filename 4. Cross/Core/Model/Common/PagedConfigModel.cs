using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Common
{
    public class PagedConfigModel
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 10;

        public string Terms { get; set; } = string.Empty;
    }
}
