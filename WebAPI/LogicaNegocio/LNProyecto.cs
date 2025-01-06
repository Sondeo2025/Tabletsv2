using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNProyecto
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNProyecto(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<Proyecto> ObtenerProyectos(int operacion,int id_usuario)
        {
            List<Proyecto> ProyectoUsuario = new List<Proyecto>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_PROYECTO", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ProyectoUsuario.Add(new Proyecto()
                        {

                            ID_PROYECTO = Convert.ToInt32(rd["ID_PROYECTO"]),
                            PROYECTO = rd["PROYECTO"].ToString(),
                            ESTADO_PROYECTO = rd["ESTADO_PROYECTO"].ToString()

                        });
                    }

                    rd.Close();
                }

                return ProyectoUsuario;
            }
        }
    }
}
