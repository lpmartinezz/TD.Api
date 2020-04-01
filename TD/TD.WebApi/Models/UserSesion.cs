using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class UserSesion
    {
        public long Idsesion { get; set; }
        public long Idusuario { get; set; }
        public DateTime HoraFin { get; set; }
        public bool EsNormalLogout { get; set; }
        public string Jwt { get; set; }
        public string Jwtheader { get; set; }
        public string Jwtpayload { get; set; }
        public string Jwtsignature { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
