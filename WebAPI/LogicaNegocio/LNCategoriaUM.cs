using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNCategoriaUM
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNCategoriaUM(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<CategoriaUM> ObtenerCategoriasUM(int operacion, int id_usuario, int id_proyecto)
        {
            List<CategoriaUM> CategoriasUMXproyecto = new List<CategoriaUM>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_CATEGORIA_UNIDADMEDIDA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CategoriasUMXproyecto.Add(new CategoriaUM()
                        {

                            ID_CATEGORIA_UNIDADMEDIDA = Convert.ToInt32(rd["ID_CATEGORIA_UNIDADMEDIDA"]),
                            ID_PROD_CATEGORIA = Convert.ToInt32(rd["ID_PROD_CATEGORIA"]),
                            ID_UNIDADMEDIDA = Convert.ToInt32(rd["ID_UNIDADMEDIDA"])

                        });
                    }

                    rd.Close();
                }

                return CategoriasUMXproyecto;
            }
        }
    }
}
