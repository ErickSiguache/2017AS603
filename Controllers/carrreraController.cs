using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class carrreraController : Controller{
        private readonly _2017AS603Context _contexto;
        public carrreraController(_2017AS603Context miContexto){
            this._contexto = miContexto;
        }

        /// <summary>
        /// Consulta de las carreras en general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/carreras")]
        public IActionResult Get()
        {
            var carreras = from e in _contexto.carreras
                           join facul in _contexto.facultades on e.facultad_id equals facul.facultad_id
                           select new {
                               e.carrera_id,
                               e.nombre_carrera,
                               facul.nombre_facultad
                           };
            if (carreras.Count() > 0)
            {
                return Ok(carreras);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de retorno de registros filtras por ID
        /// </summary>
        /// <param name="id"> Representa el valor entero del campo ID </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/carreras/{id}")]
        public IActionResult getbyId(int id)
        {
            var unaCarrera = from e in _contexto.carreras
                              join facul in _contexto.facultades on e.facultad_id equals facul.facultad_id
                              where e.carrera_id == id //Filtro por ID
                              select new
                              {
                                  e.carrera_id,
                                  e.nombre_carrera,
                                  facul.nombre_facultad
                              };

            if (unaCarrera != null)
            {
                return Ok(unaCarrera);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de insersion de datos a la tabla carreras
        /// </summary>
        /// <param name="carreraNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/carreras")]
        public IActionResult guardarCarrera([FromBody] carreras carreraNuevo)
        {
            try
            {
                IEnumerable<carreras> carreraExiste = from e in _contexto.carreras
                                                      join facul in _contexto.facultades on e.facultad_id equals facul.facultad_id
                                                      where e.nombre_carrera == carreraNuevo.nombre_carrera
                                                         && e.facultad_id == facul.facultad_id
                                                         select e;
                if (carreraExiste.Count() == 0)
                {
                    _contexto.carreras.Add(carreraNuevo);
                    _contexto.SaveChanges();
                    return Ok(carreraNuevo);
                }
                return Ok(carreraExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de modificacion de datos de la tabla carreras
        /// </summary>
        /// <param name="carreraAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/carreras")]
        public IActionResult updateCarrera([FromBody] carreras carreraAModificar)
        {
            carreras carreraExiste = (from e in _contexto.carreras
                                         where e.carrera_id == carreraAModificar.carrera_id
                                         select e).FirstOrDefault();
            if (carreraExiste is null){
                return NotFound();
            }
            carreraExiste.nombre_carrera = carreraAModificar.nombre_carrera;
            carreraExiste.facultad_id = carreraAModificar.facultad_id;

            _contexto.Entry(carreraExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(carreraExiste);

        }
    }
}
