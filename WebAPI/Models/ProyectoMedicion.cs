namespace WebAPI.Models
{
    public class ProyectoMedicion
    {
        public int ID_PROYECTO { get; set; }
        public String PROYECTO { get; set; }
        public List<Medicion> LISTA_MEDICION { get; set; }
    }
}
