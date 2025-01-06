using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class ProyectoComponentesLN
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public ProyectoComponentesLN(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<ProyectoComponenetes> ObtenerProyecto(int operacion, int id_usuario)
        {
            List<ProyectoComponenetes> ProyectoXusuario = new List<ProyectoComponenetes>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_PROYECTO", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ProyectoXusuario.Add(new ProyectoComponenetes()
                        {

                            ID_PROYECTO = Convert.ToInt32(rd["ID_PROYECTO"]),
                            ESTADO_PROYECTO = rd["ESTADO_PROYECTO"].ToString(),
                            PROYECTO = rd["PROYECTO"].ToString(),
                            LISTA_CATEGORIAS = null,
                            LISTA_EMPRESAS = null,
                            LISTA_MARCAS = null

                        });
                    }

                    rd.Close();
                }

                return ProyectoXusuario;
            }
        }
    }
}
