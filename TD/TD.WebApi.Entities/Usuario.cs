using System;

namespace TD.WebApi.Entities
{
    public class Usuario
    {
        //ID del usuario en la BD.
        public long usuarioId { get; set; }
        //Valor no relevante en el primer Sprint, sin uso, pero siempre debe ir el valor 1.
        public int rolId { get; set; }
        //Valor del Login del Usuario.
        public string codigoUsuario { get; set; }
        //Valor encriptado de la Clave Secreta. Se recomienda enviar un valor constante y no el valor real de la BD
        public string claveSecreta { get; set; }
        //Valor el eMail del usuario de la aplicación registrado.
        public string email { get; set; }
        //Datos Personales.
        public string apellidoPaterno { get; set; }
        //Datos Personales.
        public string apellidoMaterno { get; set; }
        //Datos Personales.
        public string primerNombre { get; set; }
        //No usar, en la propiedad anterior sumar todos los nombres que tenga el usuario, máximo 75 caracteres.
        public string segundoNombre { get; set; }
        //Valor de nombre de Avatar en la aplicación.
        public string alias { get; set; }
        //Dato de auditoría, columna de BD: Activo.
        public int estado { get; set; }
        //NO usar. Instanciar con valor constante.
        public string usuarioCreacion { get; set; }
        //NO usar. Instanciar con valor constante.
        public string usuarioModificacion { get; set; }
        //Dato de auditoría, columna de BD: registro.
        public DateTime fechaCreacion { get; set; }
        //NO usar. Instanciar con valor constante.
        public DateTime fechaModificacion { get; set; }
        //Trabajar en el Front, al momento de registrar, también poder registrar una empresa. Devolver el valor de la BD de la PK de la tabla Empresa.
        public int empresaId { get; set; }
    }
}