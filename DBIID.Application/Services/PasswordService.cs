using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Services
{
    public class PasswordService
    {
        public PasswordService()
        {
            
        }

        public (string, int) IncryptPassword(string password, int x)
        {
            PasswordObject passwordObject = new PasswordObject()
            {
                Password = password
            };

            string hash = string.Empty;

            string startWith = string.Empty;
            for (int i = 0; i < x; i++)
            {
                startWith += "X";
            }

            while (!hash.StartsWith(startWith))
            {
                passwordObject.Counter++;
                string obj = Newtonsoft.Json.JsonConvert.SerializeObject(passwordObject);
                hash = HashPassword(obj);
            }

            return (hash, passwordObject.Counter);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }
    }

    public class PasswordObject
    {
        public string Password { get; set; } = string.Empty;
        public int Counter { get; set; } = 1;
    }
}
