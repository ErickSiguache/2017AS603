using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class usuariosController : Controller{
        private readonly _2017AS603Context _contexto;
        public usuariosController(_2017AS603Context miContexto){
            this._contexto = miContexto;
        }

        /// <summary>
        /// Consulta de los usuarios en general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/usuarios")]
        public IActionResult Get()
        {
            var usuario = from e in _contexto.usuarios
                          join car in _contexto.carreras on e.carrera_id equals car.carrera_id
                           select new {
                               e.usuario_id,
                               e.nombre,
                               e.documento,
                               e.tipo,
                               e.carnet,
                               car.nombre_carrera
                           };
            if (usuario.Count() > 0)
            {
                return Ok(usuario);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de retorno de registros filtras por ID
        /// </summary>
        /// <param name="id"> Representa el valor entero del campo ID </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/usuarios/{id}")]
        public IActionResult getbyId(int id)
        {
            var unUsuario = from e in _contexto.usuarios
                             join carr in _contexto.carreras on e.carrera_id equals carr.carrera_id
                             where e.carrera_id == id //Filtro por ID
                             select new
                             {
                                 e.usuario_id,
                                 e.nombre,
                                 e.documento,
                                 e.tipo,
                                 e.carnet,
                                 carr.nombre_carrera
                             };

            if (unUsuario != null)
            {
                return Ok(unUsuario);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de insersion de datos a la tabla usuarios
        /// </summary>
        /// <param name="usuarioNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/usuarios")]
        public IActionResult guardarUsuario([FromBody] usuarios usuarioNuevo)
        {
            try
            {
                IEnumerable<usuarios> usuarioExiste = from e in _contexto.usuarios
                                                      join carr in _contexto.carreras on e.carrera_id equals carr.carrera_id
                                                      where e.nombre == usuarioNuevo.nombre
                                                      select e;
                if (usuarioExiste.Count() == 0)
                {
                    _contexto.usuarios.Add(usuarioNuevo);
                    _contexto.SaveChanges();
                    return Ok(usuarioNuevo);
                }
                return Ok(usuarioExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de modificacion de datos de la tabla usuario
        /// </summary>
        /// <param name="usuariosAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/usuarios")]
        public IActionResult updateUsuario([FromBody] usuarios usuariosAModificar)
        {
            usuarios usuarioExiste = (from e in _contexto.usuarios
                                         where e.usuario_id == usuariosAModificar.usuario_id
                                         select e).FirstOrDefault();
            if (usuarioExiste is null)
            {
                return NotFound();
            }
            usuarioExiste.nombre = usuariosAModificar.nombre;
            usuarioExiste.documento = usuariosAModificar.documento;
            usuarioExiste.tipo = usuariosAModificar.tipo;
            usuarioExiste.carnet = usuariosAModificar.carnet;
            usuarioExiste.carrera_id = usuariosAModificar.carrera_id;

            _contexto.Entry(usuarioExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(usuarioExiste);

        }
    }
}
