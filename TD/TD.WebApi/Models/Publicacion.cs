using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Publicacion
    {
        public long Idpublicacion { get; set; }
        public long Idformulario { get; set; }
        public long Idempresa { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string MensajeDespedida { get; set; }
        public long Usuario { get; set; }
        public bool Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
