using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public UsuarioController(IConfiguration configuration)
        {

            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        
        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion([FromBody] UsuarioRequest objDato)
        {
            try
            {
                //var data = JsonConvert.DeserializeObject<dynamic>(objDato.ToString());
                //string user = data.usuario.ToString();
                //string password = data.contrasenia.ToString();

                Usuario objUsuario = new Usuario();
                objUsuario.CUENTA_USUARIO = objDato.CUENTA_USUARIO;
                objUsuario.CLAVE = objDato.CLAVE;

                if (objUsuario.CUENTA_USUARIO == "dortiz" && objUsuario.CLAVE == "12345")
                {
                    var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("CUENTA_USUARIO", objUsuario.CUENTA_USUARIO)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        jwt.Issuer, jwt.Audience, claims, expires: DateTime.Now.AddMinutes(1),
                        signingCredentials: singIn
                    );

                    return new
                    {
                        success = true,
                        message = "Exito",
                        result = new JwtSecurityTokenHandler().WriteToken(token)
                    };

                    //return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "Credenciales incorrectas",
                        result = ""
                    };

                    //return BadRequest(new { mensaje = "Solicitud incorrecta" });
                }
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





        [HttpPost]
        [Route("login1")]
        public dynamic IniciarSesion1([FromBody] UsuarioRequest objDato)
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                //var data = JsonConvert.DeserializeObject<dynamic>(objDato.ToString());
                //string user = data.usuario.ToString();
                //string password = data.contrasenia.ToString();

                Usuario objUsuario = new Usuario();
                objUsuario.CUENTA_USUARIO = objDato.CUENTA_USUARIO;
                objUsuario.CLAVE = objDato.CLAVE;
                objUsuario.IDENTIFICADOR_TABLET = objDato.IDENTIFICADOR_TABLET;


                using (var conexion = new SqlConnection(cadenaConSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("TAB.SP_OBTENER_USUARIO_LOGIN", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CUENTA_USUARIO", SqlDbType.VarChar)).Value = objUsuario.CUENTA_USUARIO;
                    cmd.Parameters.Add(new SqlParameter("@CLAVE", SqlDbType.VarChar)).Value = objUsuario.CLAVE;
                    cmd.Parameters.Add(new SqlParameter("@IDENTIFICADOR_TABLET", SqlDbType.VarChar)).Value = objUsuario.IDENTIFICADOR_TABLET;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            usuarios.Add(new Usuario()
                            {
                                CUENTA_USUARIO = rd["CUENTA_USUARIO"].ToString(),
                                CLAVE = rd["CLAVE"].ToString(),
                                ROL = rd["ROL"].ToString(),
                                ESTADO_USUARIO = Convert.ToInt32(rd["ESTADO_USUARIO"]),
                                NOMBRES = rd["NOMBRES"].ToString(),
                                APELLIDOS = rd["APELLIDOS"].ToString(),
                                ID_USUARIOS = Convert.ToInt32(rd["ID_USUARIOS"])
                            });

                        }

                    }
                }



                if (usuarios.Count >0)
                {
                    var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("CUENTA_USUARIO", objUsuario.CUENTA_USUARIO)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token1 = new JwtSecurityToken(
                        jwt.Issuer, jwt.Audience, claims,expires: DateTime.Now.AddHours(48),
                        //jwt.Issuer, jwt.Audience, claims, expires: null,
                        signingCredentials: singIn
                    );

                    return new
                    {
                        success = true,
                        message = "Exito",
                        username = usuarios[0].CUENTA_USUARIO.ToString(),
                        nombre = usuarios[0].NOMBRES.ToString(),
                        apellido = usuarios[0].APELLIDOS.ToString(),
                        id = usuarios[0].ID_USUARIOS.ToString(),
                        token = new JwtSecurityTokenHandler().WriteToken(token1)
                    };

                    //return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "Credenciales incorrectas",
                        token = ""
                       , username = ""
                    };

                }
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message,
                    result = ""
                    ,username = ""
                };
            }

        }

    }
}
