using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class RespuestaValoracion
    {
        public long IdrespuestaValoracion { get; set; }
        public long Idcontrol { get; set; }
        public string RespuestaCorrecta { get; set; }
        public int? PuntosCorrectos { get; set; }
        public int? PuntosError { get; set; }
        public string PatterValidation { get; set; }
        public long Usuario { get; set; }
        public bool Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
