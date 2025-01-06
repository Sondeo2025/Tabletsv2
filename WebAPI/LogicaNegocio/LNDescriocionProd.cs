using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNDescripcionProd
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNDescripcionProd(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<DescriptionProd> ObtenerDescriptionProducto(int operacion, int id_usuario, int id_proyecto)
        {
            List<DescriptionProd> DescripcionProdXcategorias  = new List<DescriptionProd>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_CARATERISTICA_PROD", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;
                cmd.Parameters.Add(new SqlParameter("@CARACT", SqlDbType.Int)).Value = 1;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        DescripcionProdXcategorias.Add(new DescriptionProd()
                        {

                            ID_PROD_CATEGORIA = Convert.ToInt32(rd["ID_PROD_CATEGORIA"]),
                            ID_PROD_CARACTERISTICA = Convert.ToInt32(rd["ID_PROD_CARACTERISTICA"]),
                            DESC_PROD_CARACTERISTICA = rd["DESC_PROD_CARACTERISTICA"].ToString()

                        });
                    }

                    rd.Close();
                }

                return DescripcionProdXcategorias;
            }
        }
    }
}
