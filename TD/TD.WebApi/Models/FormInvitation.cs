using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class FormInvitation
    {
        public long IdformInvition { get; set; }
        public long Idformulario { get; set; }
        public long IduserDb { get; set; }
        public int Quniverse { get; set; }
        public int QsendInvitation { get; set; }
        public int Qresponse { get; set; }
        public int Qleft { get; set; }
        public bool? IsOnLine { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
