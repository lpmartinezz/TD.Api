using System.Collections.Generic;

namespace TD.WebApi.Entities
{
    public class ResultBaseDatoOne
    {
        public IEnumerable<HeaderRequest> Header { get; set; }
        //public IEnumerable<RowRequest> rows { get; set; }

        public IEnumerable<object> rows { get; set; }
    }
}