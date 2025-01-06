using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Claims;

namespace WebAPI.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static dynamic ValidadrToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Veirificar si se esta enviando un token valido",
                        result = ""
                    };
                }

                var usuario = identity.Claims.FirstOrDefault(x => x.Type == "CUENTA_USUARIO").Value;

                return new
                {
                    success = true,
                    message = "Exito",
                    result = usuario.ToString()
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message,
                    result = ""
                };
            }
        }
    }
}
