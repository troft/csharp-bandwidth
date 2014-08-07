using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    public class Query
    {
        public int? Page { get; set; }
        public int? Size { get; set; }

        public virtual IDictionary<string, string> ToDictionary()
        {
            var query = new Dictionary<string, string>();
            if (Page != null)
            {
                query.Add("page", Page.ToString());
            }
            if (Size != null)
            {
                query.Add("size", Size.ToString());
            }
            return query;
        }
    }
}
