using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class estado_reservaController : Controller
    {
        private readonly _2017AS603Context _contexto;
        public estado_reservaController(_2017AS603Context miContexto)
        {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo Select en general 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/estados_reserva")]
        public IActionResult Get()
        {
            var estados = from e in _contexto.estados_reserva
                          select e;

            if (estados.Count() > 0)
            {
                return Ok(estados);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de retorno de registros filtras por ID
        /// </summary>
        /// <param name="id"> Representa el valor entero del campo ID </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/estados_reserva/{id}")]
        public IActionResult getbyId(int id)
        {
            estados_reserva unEstadoRes = (from e in _contexto.estados_reserva
                                      where e.estado_res_id == id //Filtro por ID
                                      select e).FirstOrDefault();

            if (unEstadoRes != null)
            {
                return Ok(unEstadoRes);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de insersion de datos a la tabla facultades
        /// </summary>
        /// <param name="estReservaNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/estados_reserva")]
        public IActionResult guardarFacultad([FromBody] estados_reserva estReservaNuevo)
        {
            try
            {
                IEnumerable<estados_reserva> estados_reservaExiste = from e in _contexto.estados_reserva
                                                         where e.estado == estReservaNuevo.estado

                                                         select e;

                if (estados_reservaExiste.Count() == 0)
                {
                    _contexto.estados_reserva.Add(estReservaNuevo);
                    _contexto.SaveChanges();
                    return Ok(estReservaNuevo);
                }
                return Ok(estados_reservaExiste);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de modificacion de datos de la tabla facultad
        /// </summary>
        /// <param name="estados_reservaAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/estados_reserva")]
        public IActionResult updateEstados_reserva([FromBody] estados_reserva estados_reservaAModificar)
        {
            estados_reserva estados_reservaExiste = (from e in _contexto.estados_reserva
                                         where e.estado_res_id == estados_reservaAModificar.estado_res_id
                                         select e).FirstOrDefault();
            if (estados_reservaExiste is null)
            {
                return NotFound();
            }

            estados_reservaExiste.estado = estados_reservaAModificar.estado;

            _contexto.Entry(estados_reservaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(estados_reservaExiste);

        }
    }
}
