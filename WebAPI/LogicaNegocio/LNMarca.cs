using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNMarca
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNMarca(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<Marca> ObtenerMarcas(int operacion, int id_usuario, int id_proyecto)
        {
            List<Marca> MarcasXcategorias  = new List<Marca>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_MARCA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        MarcasXcategorias.Add(new Marca()
                        {

                            ID_PROD_MARCA = Convert.ToInt32(rd["ID_PROD_MARCA"]),
                            ID_PROD_EMPRESA = Convert.ToInt32(rd["ID_PROD_EMPRESA"]),
                            DESC_MARCA = rd["DESC_MARCA"].ToString()

                        });
                    }

                    rd.Close();
                }

                return MarcasXcategorias;
            }
        }
    }
}
