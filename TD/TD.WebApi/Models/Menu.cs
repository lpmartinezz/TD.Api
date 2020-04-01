using System;
using System.Collections.Generic;

namespace TD.WebApi.Models
{
    public partial class Menu
    {
        public long Idmenu { get; set; }
        public long Idparent { get; set; }
        public long Idempresa { get; set; }
        public string CodigoIdentificador { get; set; }
        public string Descripcion { get; set; }
        public string Area { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        public long Usuario { get; set; }
        public bool? Activo { get; set; }
        public DateTime Registro { get; set; }
    }
}
