using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNUnidadMedida
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNUnidadMedida(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<UnidadMedida> ObtenerUnidadMedida( )
        {
            List<UnidadMedida> UnidadMedida = new List<UnidadMedida>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_UNIDAD_MEDIDA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        UnidadMedida.Add(new UnidadMedida()
                        {

                            ID_UNIDADMEDIDA = Convert.ToInt32(rd["ID_UNIDADMEDIDA"]),
                            COD_UNIDADMEDIDA = rd["COD_UNIDADMEDIDA"].ToString(),
                            DESC_UNIDADMEDIDA = rd["DESC_UNIDADMEDIDA"].ToString()

                        });
                    }

                    rd.Close();
                }

                return UnidadMedida;
            }
        }
    }
}
