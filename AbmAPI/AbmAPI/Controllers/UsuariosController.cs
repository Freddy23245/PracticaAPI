using AbmAPI.AccesoDatos;
using AbmAPI.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AbmAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DatosUsuario _datosUsuario;
        private readonly JwtManager _jwtManager;
        public UsuariosController(DatosUsuario datosUsuario)
        {
            _datosUsuario = datosUsuario;
            _jwtManager = new JwtManager();
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        
        public IActionResult Login([FromBody] Login log)
        {
            if (log == null)
                return BadRequest();
            if(string.IsNullOrEmpty(log.usuario) || string.IsNullOrEmpty(log.password))
                return BadRequest();
            // Ejemplo básico sin validación de la base de datos
            if (_datosUsuario.Login(log))
            {
                var token = _jwtManager.GenerarToken(log);
                return Ok(new {log.usuario, token });
            }
         

            return Unauthorized();
        }
        [HttpGet]
        [Authorize]
        public  IActionResult ObtenerUsuarios()
        {
            var usuarios = _datosUsuario.ObtenerUsuarios();
            return Ok(usuarios);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Agregar([FromBody] Usuarios usuarios)
        {
                var usu =  await _datosUsuario.Agregar(usuarios);
            return Ok(usu);

        }
        [HttpPut]
        [Authorize]
        public async Task Editar([FromBody] Usuarios usuarios)
        {
            await _datosUsuario.Editar(usuarios);
        }
        [HttpDelete]
        [Authorize]
        public async Task Eliminar([FromBody] Usuarios usuarios)
        {
            await _datosUsuario.Eliminar(usuarios);
        }


    }
}
