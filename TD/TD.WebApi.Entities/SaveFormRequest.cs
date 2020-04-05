using System.Collections.Generic;

namespace TD.WebApi.Entities
{
    public class SaveFormRequest
    {
        //Usuario.
        public string idusuario { get; set; }
        //Empresa asociada al usuario.
        public string idempresa { get; set; }
        //Valor PK de la tabla Formularios.
        public string idform { get; set; }
        public string token { get; set; }

        public IEnumerable<Respuesta> controls { get; set; }
    }
}