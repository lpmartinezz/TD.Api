using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class UserDbrows
    {
        public long IduserDbrow { get; set; }
        public long IduserDb { get; set; }
        public string UserDataRow { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
