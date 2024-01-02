using AbmAPI.AccesoDatos;
using AbmAPI.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbmAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DatosProducto _datosProducto;
        private readonly JwtManager _jwtManager;
        public ProductosController(DatosProducto datosProducto)
        {
            _datosProducto = datosProducto;
            _jwtManager = new JwtManager();
        }

        [HttpGet("ObtenerProductos")]
        public IActionResult ObtenerProductos()
        {
            var productos = _datosProducto.ObtenerProductos();
            return Ok(productos);
        }

    }
}
