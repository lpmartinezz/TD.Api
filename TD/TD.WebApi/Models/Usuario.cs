using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Usuario
    {
        public long Idusuario { get; set; }
        public long Idempresa { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Apodo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public long Usuario1 { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
