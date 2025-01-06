using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNCategoria
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNCategoria(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<Categoria> ObtenerCategorias(int operacion, int id_usuario, int id_proyecto)
        {
            List<Categoria> CategoriasXproyecto = new List<Categoria>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_CATEGORIA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        CategoriasXproyecto.Add(new Categoria()
                        {

                            ID_PROD_CATEGORIA = Convert.ToInt32(rd["ID_PROD_CATEGORIA"]),
                            DESC_CATEGORIA = rd["DESC_CATEGORIA"].ToString()

                        });
                    }

                    rd.Close();
                }

                return CategoriasXproyecto;
            }
        }
    }
}
