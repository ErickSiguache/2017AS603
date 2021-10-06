using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class reservasController : Controller
    {
        private readonly _2017AS603Context _contexto;
        public reservasController(_2017AS603Context miContexto)
        {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/reservas")]
        public IActionResult Get()
        {
            var reservas = from e in _contexto.reservas
                           join equi in _contexto.equipos on e.equipo_id equals equi.id_equipos
                           join usr in _contexto.usuarios on e.usuario_id equals usr.usuario_id
                           join estR in _contexto.estados_reserva on e.estado_reserva_id equals estR.estado_res_id
                           select new
                           {
                               e.reserva_id,
                               equipoNombre = equi.nombre,
                               userNombre = usr.nombre,
                               e.fecha_salida,
                               e.hora_salida,
                               e.tiempo_reserva,
                               estR.estado,
                               e.fecha_retorno,
                               e.hora_retorno
                           };
            if (reservas.Count() > 0)
            {
                return Ok(reservas);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de retorno de registros filtras por ID
        /// </summary>
        /// <param name="id"> Representa el valor entero del campo ID </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/reservas/{id}")]
        public IActionResult getbyId(int id)
        {
            var reservas = from e in _contexto.reservas
                           join equi in _contexto.equipos on e.equipo_id equals equi.id_equipos
                           join usr in _contexto.usuarios on e.usuario_id equals usr.usuario_id
                           join estR in _contexto.estados_reserva on e.estado_reserva_id equals estR.estado_res_id
                           where e.reserva_id == id //Filtro por ID
                           select new
                           {
                               e.reserva_id,
                               equipoNombre = equi.nombre,
                               userNombre = usr.nombre,
                               e.fecha_salida,
                               e.hora_salida,
                               e.tiempo_reserva,
                               estR.estado,
                               e.fecha_retorno,
                               e.hora_retorno
                           };

            if (reservas != null)
            {
                return Ok(reservas);
            }
            return NotFound();
        }

        /// <summary>
        /// Metodo de insersion de datos a la tabla reserva
        /// </summary>
        /// <param name="reservaNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/reservas")]
        public IActionResult guardarReserva([FromBody] reservas reservaNuevo)
        {
            try
            {
                ///"e" representa un alias para el listado de equipos
                IEnumerable<reservas> reservaExiste = from e in _contexto.reservas
                                                    where e.equipo_id == reservaNuevo.equipo_id
                                                    && e.fecha_retorno == reservaNuevo.fecha_retorno
                                                    select e;

                ///Realiza una compracion de que sea diferente de 0 en la busqueda si existe que se realiza
                ///en la consulta y si no existe nada deja insertarlo 
                if (reservaExiste.Count() == 0)
                {
                    _contexto.reservas.Add(reservaNuevo);
                    _contexto.SaveChanges();
                    return Ok(reservaNuevo);
                }
                return Ok(reservaExiste);
            }
            ///de no ser asi lo envia al catch y muestra un error
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de modificacion de datos de la tabla reservas
        /// </summary>
        /// <param name="reservasAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/reservas")]
        public IActionResult updateReservas([FromBody] reservas reservasAModificar)
        {
            reservas reservasExiste = (from e in _contexto.reservas
                                    where e.reserva_id == reservasAModificar.reserva_id
                                    select e).FirstOrDefault();
            if (reservasExiste is null)
            {
                return NotFound();
            }

            reservasExiste.equipo_id = reservasAModificar.equipo_id;
            reservasExiste.usuario_id = reservasAModificar.usuario_id;
            reservasExiste.fecha_salida = reservasAModificar.fecha_salida;
            reservasExiste.hora_salida = reservasAModificar.hora_salida;
            reservasExiste.tiempo_reserva = reservasAModificar.tiempo_reserva;
            reservasExiste.estado_reserva_id = reservasAModificar.estado_reserva_id;
            reservasExiste.fecha_retorno = reservasAModificar.fecha_retorno;
            reservasExiste.hora_retorno = reservasAModificar.hora_retorno;

            _contexto.Entry(reservasExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(reservasExiste);

        }


        /// <summary>
        /// Metodo de retorno de registros filtras por ID
        /// </summary>
        /// <param name="id"> Representa el valor entero del campo ID </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/reservas/estadoID/{idEst}")]
        public IActionResult getbyIdEst(int idEst)
        {
            var resEstID = from e in _contexto.reservas
                           join equi in _contexto.equipos on e.equipo_id equals equi.id_equipos
                           join usr in _contexto.usuarios on e.usuario_id equals usr.usuario_id
                           join estR in _contexto.estados_reserva on e.estado_reserva_id equals estR.estado_res_id
                           where e.estado_reserva_id == idEst //Filtro por ID
                           select new
                           {
                               e.reserva_id,
                               equipoNombre = equi.nombre,
                               userNombre = usr.nombre,
                               e.fecha_salida,
                               e.hora_salida,
                               e.tiempo_reserva,
                               estR.estado,
                               e.fecha_retorno,
                               e.hora_retorno
                           };

            if (resEstID != null)
            {
                return Ok(resEstID);
            }
            return NotFound();
        }
    }
}
