using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using WebAPI.Context;
using WebAPI.LogicaNegocio;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("proyecto")]
    public class ProyectoController : ControllerBase
    {

        public IConfiguration _configuration;
        private readonly string cadenaConSql;
        private readonly ProyectoComponentesLN _proyectoLn;
        private readonly LNCategoria _lnCategoria;
        private readonly LNEmpresa _lnEmpresa;
        private readonly LNMarca _lnMarca;
        private readonly LNProducto _lnProducto;
        private readonly LNDescripcionProd _lndDescripcionProd;
        private readonly LNProyectoCategoria _lndProyectoCategoria;
        private readonly LNUnidadMedida _lndUnidadMedida;
        private readonly LNCategoriaUM _lnCategoriaUM;
        private readonly LNProyectoMedicion _lnMedicionPoryecto;
        private readonly LNProyecto _lnPoryecto;
        private readonly LNMedicion _lnMedicion;
        private readonly LNDatosFechaEntrega _lnFechaDatos;

        public ProyectoController(IConfiguration configuration)
        {

            _configuration = configuration;
            cadenaConSql = configuration.GetConnectionString("BD_CONEXION_SQL_SERVER");
            _proyectoLn = new ProyectoComponentesLN(_configuration);
            _lnCategoria = new LNCategoria(_configuration);
            _lnEmpresa = new LNEmpresa(_configuration);
            _lnMarca = new LNMarca(_configuration);
            _lnProducto = new LNProducto(_configuration);
            _lndDescripcionProd = new LNDescripcionProd(_configuration);
            _lndProyectoCategoria = new LNProyectoCategoria(_configuration);
            _lndUnidadMedida = new LNUnidadMedida(_configuration);
            _lnCategoriaUM = new LNCategoriaUM(_configuration);
            _lnMedicionPoryecto = new LNProyectoMedicion(_configuration);
            _lnPoryecto = new LNProyecto(_configuration);
            _lnMedicion = new LNMedicion(_configuration);
            _lnFechaDatos = new LNDatosFechaEntrega(_configuration);
            
        }




        [HttpGet]
        //[Route("ComponentesProyecto/{operacion }/{ id_usuario}")]
        [Route("ComponentesProyecto")]

        //    public IActionResult ComponentesProyecto(int operacion, int id_usuario)
        public IActionResult ComponentesProyecto  ([FromBody] UsuarioRequest1 objDato)
        {

            var id_usuario = objDato.ID_PROYECTO;

            var operacion = objDato.OPERACION;

            List <ProyectoComponenetes> LisProyectosXusuario = new List<ProyectoComponenetes>();
            List<Categoria> ListCategoriasXProyecto = new List<Categoria>();
            List<Empresa> ListEmpresasXProyecto = new List<Empresa>();
            List<Marca> ListMarcasXProyecto = new List<Marca>();
            List<Producto> ListProductosXProyecto = new List<Producto>();
            List<DescriptionProd> ListDescripcionProdXProyecto = new List<DescriptionProd>();
            List<ProyectoCategoria> ListProyectoCategoria = new List<ProyectoCategoria>();
            List<UnidadMedida> ListUnidadMedida = new List<UnidadMedida>();
            List<CategoriaUM> ListCategoriasUM = new List<CategoriaUM>();
            List<DatosFechaEntrega> ListFechaDatos = new List<DatosFechaEntrega>();



            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var token = Jwt.ValidadrToken(identity);

                if (!token.success) return token;

                LisProyectosXusuario = _proyectoLn.ObtenerProyecto(operacion,id_usuario);

                if (LisProyectosXusuario.Count > 0)
                {
                    foreach (var item in LisProyectosXusuario)
                    {


                        ListCategoriasXProyecto = _lnCategoria.ObtenerCategorias(operacion, id_usuario,Convert.ToInt32(item.ID_PROYECTO));
                        if (ListCategoriasXProyecto.Count > 0)
                        {
                            item.LISTA_CATEGORIAS = ListCategoriasXProyecto;
                        }


                        ListEmpresasXProyecto = _lnEmpresa.ObtenerEmpresas(operacion, id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListEmpresasXProyecto.Count > 0)
                        {
                            item.LISTA_EMPRESAS = ListEmpresasXProyecto;
                        }


                        ListMarcasXProyecto = _lnMarca.ObtenerMarcas(operacion, id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListMarcasXProyecto.Count > 0)
                        {
                            item.LISTA_MARCAS = ListMarcasXProyecto;
                        }


                        ListProductosXProyecto = _lnProducto.ObtenerProductos(operacion, id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListProductosXProyecto.Count > 0)
                        {
                            item.LISTA_PRODUCTOS = ListProductosXProyecto;
                        }

                        ListDescripcionProdXProyecto = _lndDescripcionProd.ObtenerDescriptionProducto(operacion, id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListDescripcionProdXProyecto.Count > 0)
                        {
                            item.LISTA_CARACTERISTICAS_PROD = ListDescripcionProdXProyecto;
                        }


                        ListProyectoCategoria = _lndProyectoCategoria.ObtenerProyectoCategoria(operacion, id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListProyectoCategoria.Count > 0)
                        {
                            item.LISTA_PRODUCTO_CATEGORIA = ListProyectoCategoria;
                        }


                        ListUnidadMedida = _lndUnidadMedida.ObtenerUnidadMedida();
                        if (ListUnidadMedida.Count > 0)
                        {
                            item.LISTA_UNIDADMEDIDA = ListUnidadMedida;
                        }

                        ListCategoriasUM = _lnCategoriaUM.ObtenerCategoriasUM(operacion, id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListCategoriasUM.Count > 0)
                        {
                            item.LISTA_CATEGORIASUM = ListCategoriasUM;
                        }

                        ListFechaDatos = _lnFechaDatos.ObtenerFechaDatos( id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                        if (ListCategoriasUM.Count > 0)
                        {
                            item.LISTA_DATOS_ENTREGA = ListFechaDatos;
                        }
                    }
                }


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = LisProyectosXusuario });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = LisProyectosXusuario });
            }
        }




        [HttpGet]
        [Route("MedicionXproyecto/{id_usuario:int}")]
        public IActionResult MedicionXproyecto(int id_usuario)
        {
            List<ProyectoMedicion> ListMedicionProyecto = new List<ProyectoMedicion>();
            List<Proyecto> ListProyecto = new List<Proyecto>();
            List<Medicion> ListMedicion = new List<Medicion>();



            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                var token = Jwt.ValidadrToken(identity);

                if (!token.success) return token;

                ListProyecto = _lnPoryecto.ObtenerProyectos(1,id_usuario);

                if (ListProyecto.Count > 0)
                {
                    
                    foreach (var item in ListProyecto)
                    {
                        ListMedicionProyecto = _lnMedicionPoryecto.ObtenerMedicionesProyectos(id_usuario, Convert.ToInt32(item.ID_PROYECTO));

                        
                        if (ListMedicionProyecto.Count > 0)
                        {
                            foreach (var item1 in ListMedicionProyecto)
                            {
                                ListMedicion = _lnMedicion.ObtenerMedicionesXusuario(id_usuario, Convert.ToInt32(item.ID_PROYECTO));
                                if (ListMedicion.Count > 0)
                                {
                                    item1.LISTA_MEDICION = ListMedicion;
                                }
                            }
                            
                        }
                    }
                }


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Ok", response = ListMedicionProyecto });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = ListMedicionProyecto });
            }
        }
    }
}
