using Microsoft.EntityFrameworkCore;

namespace WebAPI.Context
{
    public class Conexion : DbContext
    {
        public Conexion(DbContextOptions<Conexion> options) : base(options)
        {

        }
    }
}
