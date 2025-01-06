using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNProducto
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNProducto(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<Producto> ObtenerProductos(int operacion, int id_usuario, int id_proyecto)
        {
            List<Producto> ProductosXproyecto  = new List<Producto>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_PRODUCTO", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ProductosXproyecto.Add(new Producto()
                        {

                            ID_PRODUCTO = Convert.ToInt32(rd["ID_PRODUCTO"]),
                            ID_PROD_CATEGORIA = Convert.ToInt32(rd["ID_PROD_CATEGORIA"]),
                            ID_PROD_MARCA = Convert.ToInt32(rd["ID_PROD_MARCA"]),
                            ID_PROD_EMPRESA = Convert.ToInt32(rd["ID_PROD_EMPRESA"]),
                            PROD_DESC = rd["PROD_DESC"].ToString(),
                            ID_PROD_CAR1 = Convert.ToInt32(rd["ID_PROD_CAR1"]),

                        });
                    }

                    rd.Close();
                }

                return ProductosXproyecto;
            }
        }
    }
}
