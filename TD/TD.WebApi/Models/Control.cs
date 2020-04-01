using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Control
    {
        public long Idcontrol { get; set; }
        public long Idformulario { get; set; }
        public string Htmlid { get; set; }
        public string Htmlname { get; set; }
        public long IdtipoControl { get; set; }
        public string Texto { get; set; }
        public string Descripcion { get; set; }
        public string Opciones { get; set; }
        public long IdtipoRespuesta { get; set; }
        public bool EsRequerido { get; set; }
        public bool? EsRespuestaLarga { get; set; }
        public long? IdtipoSimbolo { get; set; }
        public long? Idnivel { get; set; }
        public long? IdtipoRestriccion { get; set; }
        public string IntervaloInicio { get; set; }
        public string IntervaloFin { get; set; }
        public int? Orden { get; set; }
        public int FormularioVersion { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
