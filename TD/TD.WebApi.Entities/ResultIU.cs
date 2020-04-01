namespace TD.WebApi.Entities
{
    public class ResultIU
    {
        //ID del formulario Insertado/Actualizado
        public int idForm { get; set; }
        //Resultado de transacción al intentar Insertado/Actualizado
        public string mensaje { get; set; }
    }
}