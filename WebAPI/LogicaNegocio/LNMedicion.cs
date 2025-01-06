using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNMedicion
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNMedicion(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<Medicion> ObtenerMedicionesXusuario(int id_usuario, int id_proyecto)
        {
            List<Medicion> MedicionXusuario = new List<Medicion>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_MEDICION_USUARIO", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
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

                    rd.Close();
                }

                return MedicionXusuario;
            }
        }
    }
}
