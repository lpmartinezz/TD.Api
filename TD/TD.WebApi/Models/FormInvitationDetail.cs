using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class FormInvitationDetail
    {
        public long Idfidetail { get; set; }
        public long IdformInvition { get; set; }
        public bool IsAnswered { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Idcard { get; set; }
        public string Mobile { get; set; }
        public string Urlqs { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
