namespace WebAPI.Models
{
    public class ProyectoComponenetes
    {
        public int ID_PROYECTO { get; set; }
        public String ESTADO_PROYECTO { get; set; }
        public String PROYECTO { get; set; }
        public List<Categoria> LISTA_CATEGORIAS { get; set; }
        public List<Empresa> LISTA_EMPRESAS { get; set; }
        public List<Marca> LISTA_MARCAS { get; set; }
        public List<Producto> LISTA_PRODUCTOS { get; set; }
        public List<DescriptionProd> LISTA_CARACTERISTICAS_PROD { get; set; }
        public List<ProyectoCategoria> LISTA_PRODUCTO_CATEGORIA { get; set; }
        public List<UnidadMedida> LISTA_UNIDADMEDIDA { get; set; }
        public List<CategoriaUM> LISTA_CATEGORIASUM { get; set; }

        public List<DatosFechaEntrega> LISTA_DATOS_ENTREGA { get; set; }
    }
}
