using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.LogicaNegocio
{
    public class LNEmpresa
    {
        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        public LNEmpresa(IConfiguration configuration)
        {
            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
        }

        public List<Empresa> ObtenerEmpresas(int operacion, int id_usuario, int id_proyecto)
        {
            List<Empresa> EmpresasXcategorias  = new List<Empresa>();

            using (var conexion = new SqlConnection(cadenaConSql))
            {
                conexion.Open();
                var cmd = new SqlCommand("TAB.SP_EMPRESA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@OPERACION", SqlDbType.Int)).Value = operacion;
                cmd.Parameters.Add(new SqlParameter("@ID_USUARIO", SqlDbType.Int)).Value = id_usuario;
                cmd.Parameters.Add(new SqlParameter("@ID_PROYECTO", SqlDbType.Int)).Value = id_proyecto;

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        EmpresasXcategorias.Add(new Empresa()
                        {

                            ID_PROD_EMPRESA = Convert.ToInt32(rd["ID_PROD_EMPRESA"]),
                            DESC_EMPRESA = rd["DESC_EMPRESA"].ToString()

                        });
                    }

                    rd.Close();
                }

                return EmpresasXcategorias;
            }
        }
    }
}
