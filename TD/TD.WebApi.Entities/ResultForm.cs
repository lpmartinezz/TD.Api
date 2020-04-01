using System.Collections.Generic;

namespace TD.WebApi.Entities
{
    public class ResultForm
    {
        //Valor PK de la tabla Formularios.
        public string idform { get; set; }
        //Valor del título del formulario.
        public string titulo { get; set; }
        //Valor de la fecha de vigencia-mover a instanciar valor en la publicación.
        public string fecha_vigencia { get; set; }
        //Valor de la finalidad de la definición del formulario.
        public string comentario { get; set; }
        //Estado del registro del formulario, dato de auditoría tomado de la columna Activo.
        public string estado { get; set; }
        //Usuario propietario (creador) del formulario.
        public string idusuario { get; set; }
        //Empresa asociada al usuario.
        public string idempresa { get; set; }

        public IEnumerable<Control> controls { get; set; }
    }
}