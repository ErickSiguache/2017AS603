using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class estados_equipoController : ControllerBase{
        private readonly _2017AS603Context _contexto;
        public estados_equipoController(_2017AS603Context miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo Select en general de la tabla estado_equipo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/estados_equipo")]
        public IActionResult Get()
        {
            ///"e" representa un alias para el listado de marcas
            IEnumerable<estados_equipo> estados_equipoList = from e in _contexto.estados_equipo
                                                     select e;

            ///Verifica que la lista no este vacia y retorna los datos
            if (estados_equipoList.Count() > 0)
            {
                return Ok(estados_equipoList);
            }
            ///De estar vacia muestra un error
            return NotFound();
        }

        /// <summary>
        /// Metodo de Busqueda por ID en la tabla estado_equipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/estados_equipo/{id}")]
        public IActionResult getbyId(int id)
        {
            ///"e" representa un alias para el listado de estado de equipo
            estados_equipo unEstado = (from e in _contexto.estados_equipo
                               where e.id_estados_equipo == id //Filtro por ID
                               select e).FirstOrDefault();

            ///verifica que el ID preguntado tenga datos y luego los retorna
            if (unEstado != null)
            {
                return Ok(unEstado);
            }
            ///De ser nulo (No tener ningun dato o no exister mostrara un error)
            return NotFound();
        }

        /// <summary>
        /// Metodo de Insercion de Datos a la tabla estado_equipo
        /// </summary>
        /// <param name="estado_equipoNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/estados_equipo")]
        public IActionResult guardarEstado_equipo([FromBody] estados_equipo estado_equipoNuevo)
        {
            try
            {
                ///"e" representa un alias para el listado de estados
                IEnumerable<estados_equipo> estados_equipoExiste = from e in _contexto.estados_equipo
                                                  where e.descripcion == estado_equipoNuevo.descripcion
                                                  select e;

                ///Realiza una compracion de que sea diferente de 0 en la busqueda si existe que se realiza
                ///en la consulta y si no existe nada deja insertarlo 
                if (estados_equipoExiste.Count() == 0)
                {
                    _contexto.estados_equipo.Add(estado_equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(estado_equipoNuevo);
                }
                return Ok(estados_equipoExiste);
            }
            ///de no ser asi lo envia al catch y muestra un error
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de Modificacion de datos en la tabla estado_equipo
        /// </summary>
        /// <param name="estadoAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/estados_equipo")]
        public IActionResult updateEstado([FromBody] estados_equipo estadoAModificar)
        {
            ///"e" representa un alias para el listado de esatado_equipo donde se comprara la seleccion para
            ///su modidicacion
            estados_equipo estadoExiste = (from e in _contexto.estados_equipo
                                  where e.id_estados_equipo == estadoAModificar.id_estados_equipo
                                  select e).FirstOrDefault();
            if (estadoExiste is null)
            {
                return NotFound();
            }

            /// Donde se identifica el valor que se esta insertando en memoria y donde se
            ///insertara en la base de datos
            estadoExiste.descripcion = estadoAModificar.descripcion;
            estadoExiste.estado = estadoAModificar.estado;

            _contexto.Entry(estadoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(estadoExiste);

        }
    }
}