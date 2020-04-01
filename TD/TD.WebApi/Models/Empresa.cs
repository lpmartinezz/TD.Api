using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Empresa
    {
        public long Idempresa { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Idfiscal { get; set; }
        public long Idpais { get; set; }
        public long Idresponsable { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
