using System.Security.Cryptography;
using System.Text;

namespace TD.WebApi.Logic
{
    public class UrlQS
    {

        public string GenerarQS(string Name, string LasName, string Email, string Idcard, string Mobile)
        {
            string CodigoQS = ComputeSha512Hash(Name + LasName + Email + Idcard + Mobile);
            return CodigoQS;
        }

        public static string ComputeSha512Hash(string rawData)
        {
            // Create a SHA256  
            using (SHA512 sha256Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                // Convert byte array to a string  
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }

}