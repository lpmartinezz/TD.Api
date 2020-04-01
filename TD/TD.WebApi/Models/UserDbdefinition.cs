using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class UserDbdefinition
    {
        public long IduserDb { get; set; }
        public long Idempresa { get; set; }
        public long Idusuario { get; set; }
        public string Descripcion { get; set; }
        public string Header { get; set; }
        public bool EsPublicada { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
