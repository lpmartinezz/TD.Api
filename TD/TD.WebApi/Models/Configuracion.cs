using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Configuracion
    {
        public long Idconfiguracion { get; set; }
        public string Clave { get; set; }
        public string Valor { get; set; }
        public string Grupo { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
        public long? Usuario { get; set; }
    }
}
