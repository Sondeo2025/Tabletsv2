using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNDatosFechaEntrega
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNDatosFechaEntrega(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<DatosFechaEntrega> ObtenerFechaDatos(  int id_usuario, int id_proyecto)
        {
            List<DatosFechaEntrega> FechaDatos = new List<DatosFechaEntrega>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_DATOS_FECHA_ENTREGA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        FechaDatos.Add(new DatosFechaEntrega()
                        {

                            ID_USUARIO = Convert.ToInt32(rd["ID_USUARIO"]),
                            ID_PROYECTO = rd["ID_PROYECTO"].ToString()

                        });
                    }

                    rd.Close();
                }

                return FechaDatos;
            }
        }
    }
}
