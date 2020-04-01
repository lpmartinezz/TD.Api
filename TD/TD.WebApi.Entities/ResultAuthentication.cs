using System.Collections.Generic;

namespace TD.WebApi.Entities
{
    public class ResultAuthentication
    {
        public ValidaUsuario validaUsuario { get; set; }
        public Usuario usuario { get; set; }
        public IEnumerable<Menu> menu { get; set; }
    }
}