using System.Collections.Generic;

namespace TD.WebApi.Entities
{
    public class SendBaseDatoRequest
    {
        public string idUsuario { get; set; }
        public string idEmpresa { get; set; }
        public IEnumerable<HeaderRequest> Header { get; set; }
        public IEnumerable<RowRequest> rows { get; set; }
    }
}