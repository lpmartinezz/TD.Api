using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Respuesta
    {
        public long Idrespuesta { get; set; }
        public long Idformulario { get; set; }
        public long Idcontrol { get; set; }
        public string Urlqs { get; set; }
        public string Respuesta1 { get; set; }
        public long Usuario { get; set; }
        public bool Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
