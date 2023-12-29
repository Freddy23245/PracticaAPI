using AbmAPI.AccesoDatos;
using AbmAPI.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbmAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly DatosCliente _datosCliente;
        private readonly JwtManager _jwtManager;
        public ClientesController(DatosCliente datosCliente)
        {
            _datosCliente = datosCliente;
            _jwtManager = new JwtManager();
        }
        [HttpGet("ObtenerClientes")]
        [Authorize]

        public IActionResult ObtenerClientes()
        {
            var clientes = _datosCliente.ObtenerCliente();
            return Ok(clientes);
        }
        [HttpPost("AgregarClientes")]
        [Authorize]
        public async Task AgregarClientes([FromBody] Clientes cliente)
        {
            await _datosCliente.AgregarCliente(cliente);
        }
        [HttpPut("EditarClientes")]
        [Authorize]
        public async Task EditarClientes([FromBody] Clientes cliente)
        {
              await _datosCliente.EditarCliente(cliente);
        }
        [HttpDelete("EliminarClientes")]
        [Authorize]
        public async Task Eliminar([FromBody] Clientes cliente)
        {
            await _datosCliente.EliminarCliente( cliente);
        }

    }
}
