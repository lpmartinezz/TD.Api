namespace TD.WebApi.Entities
{
    public class ValidaUsuario
    {
        //ID del usuario en la BD.
        public int identificador { get; set; }
        //Resultado textual de la consulta de la autenticación.
        public string mensaje { get; set; }
        //Valore de login autenticado
        public string codigoUsuario { get; set; }
        //Resultado si fue o no correctamente autenticado.
        public bool isAutenticado { get; set; }
    }
}