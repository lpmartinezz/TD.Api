using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class UserEventLog
    {
        public long IdeventLog { get; set; }
        public long IdeventType { get; set; }
        public long Idsesion { get; set; }
        public string LastValue { get; set; }
        public string TextMessage { get; set; }
        public string UserTableName { get; set; }
        public long? ValuePk { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
