using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class CanalComunicacion
    {
        public long Idcanal { get; set; }
        public long IdtipoCanal { get; set; }
        public long Idusuario { get; set; }
        public string Valor { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
        public long Usuario { get; set; }
    }
}
