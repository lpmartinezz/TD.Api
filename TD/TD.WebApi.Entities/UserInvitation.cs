namespace TD.WebApi.Entities
{
    public class UserInvitation
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public string dni { get; set; }
        public string celular { get; set; }

        public ResultForm form { get; set; }
    }
}