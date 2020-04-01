namespace TD.WebApi.Entities
{
    public class Menu
    {
        //ID del Menú
        public long menuId { get; set; }
        //ID del Menú, no se usa y cuando se use, sería un problema en el pintado en la vista.
        public long menuPadreId { get; set; }
        //Valor del menú que se pinta.
        public string codigoIdentificador { get; set; }
        //Valor descriptivo del Menú, no se tiene claro si se pinta o cómo dentro de la vista en la aplicación.
        public string descripcion { get; set; }
        //Nombre del módulo, los valores los debe de proporcionar César.
        public string area { get; set; }
        //Nombre del controlador, los valores los debe de proporcionar César.
        public string controlador { get; set; }
        //Nombre del objeto json que responde: response_Autentificacion, response_GetAllFormularios, response_GetOneForm, 
        //response_InsertForm, response_UpdateForm. Además de los demás objetos que a la fecha no han sido alcanzados en la documentación
        public string accion { get; set; }
        //Nombre del archivo físico dentro del proyecto aplicación.
        public string icono { get; set; }
        //No usado, pero instanciar siempre en 1.
        public int orden { get; set; }
        // Estado siempre en true. en las pruebas verificar qué pasa si viaja en false.
        public bool? estado { get; set; }
    }
}