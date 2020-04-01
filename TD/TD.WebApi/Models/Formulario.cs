using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Formulario
    {
        public long Idformulario { get; set; }
        public long IdtipoFormulario { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public long Idusuario { get; set; }
        public long Idempresa { get; set; }
        public DateTime FechaVigencia { get; set; }
        public int Version { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
