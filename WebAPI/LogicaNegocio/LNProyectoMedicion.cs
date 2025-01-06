using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNProyectoMedicion
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNProyectoMedicion(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<ProyectoMedicion> ObtenerMedicionesProyectos(int id_usuario, int id_proyecto)
        {
            List<ProyectoMedicion> listaProyectoMedicion = new List<ProyectoMedicion>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_PROYECTO_MEDICION_USUARIO", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        listaProyectoMedicion.Add(new ProyectoMedicion()
                        {

                            ID_PROYECTO = Convert.ToInt32(rd["ID_PROYECTO"]),
                            PROYECTO = rd["PROYECTO"].ToString(),
                            LISTA_MEDICION=null

                        });
                    }

                    rd.Close();
                }

                return listaProyectoMedicion;
            }
        }
    }
}
