using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNProyectoCategoria
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNProyectoCategoria(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<ProyectoCategoria> ObtenerProyectoCategoria(int operacion, int id_usuario, int id_proyecto)
        {
            List<ProyectoCategoria> ProyectoCategoria  = new List<ProyectoCategoria>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_PROYECTO_CATEGORIA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ProyectoCategoria.Add(new ProyectoCategoria()
                        {

                            ID_PROD_CATEGORIA = Convert.ToInt32(rd["ID_PROD_CATEGORIA"]),
                            ID_PROYECTO = Convert.ToInt32(rd["ID_PROYECTO"])

                        });
                    }

                    rd.Close();
                }

                return ProyectoCategoria;
            }
        }
    }
}
