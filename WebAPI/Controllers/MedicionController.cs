using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("medicion")]
    public class MedicionController : ControllerBase
    {

        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public MedicionController(IConfiguration configuration)
        {

            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        [HttpGet]
        [Route("MedicionXusuario/{id_usuario:int}")]
        public IActionResult MedicionXusuario(int id_usuario)
        { 
            List<Medicion> MedicionXusuario = new List<Medicion>();
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var token = Jwt.ValidadrToken(identity);

                if (!token.success) return token;

                using (var conexion = new SqlConnection(cadenaConSql))
                { 
                    conexion.Open();
                    var cmd = new SqlCommand("TAB.SP_MEDICION_USUARIO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;

                    using (var rd = cmd.ExecuteReader() )
                    {
                        while (rd.Read())
                        {
                            MedicionXusuario.Add(new Medicion() 
                            {
                                
                                ID_MEDICION = Convert.ToInt32(rd["ID_MEDICION"]),
                                MEDICION = rd["MEDICION"].ToString(),
                                ESTADO_MEDICION = rd["ESTADO_MEDICION"].ToString(),
                                ID_MEDICION_REF = Convert.ToInt32(rd["ID_MEDICION_REF"])
                            });

                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new {mensaje = "Ok" , response = MedicionXusuario });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = MedicionXusuario });
            }
        }
    }
}
