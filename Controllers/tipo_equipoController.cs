using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class tipo_equipoController : ControllerBase{
        private readonly _2017AS603Context _contexto;
        public tipo_equipoController(_2017AS603Context miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo Select en general de la tabla tipos de equipo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/tipo_equipo")]
        public IActionResult Get()
        {
            ///"e" representa un alias para el listado de tipos
            IEnumerable<tipo_equipo> tiposList = from e in _contexto.tipo_equipo
                                                 select e;

            ///Verifica que la lista no este vacia y retorna los datos
            if (tiposList.Count() > 0)
            {
                return Ok(tiposList);
            }
            ///De estar vacia muestra un error
            return NotFound();
        }

        /// <summary>
        /// Metodo de Busqueda por ID en la tabla tipo_equipo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/tipo_equipo/{id}")]
        public IActionResult getbyId(int id)
        {
            ///"e" representa un alias para el listado de tipos
            tipo_equipo unTipo = (from e in _contexto.tipo_equipo
                               where e.id_tipo_equipo == id //Filtro por ID
                               select e).FirstOrDefault();

            ///verifica que el ID preguntado tenga datos y luego los retorna
            if (unTipo != null)
            {
                return Ok(unTipo);
            }
            ///De ser nulo (No tener ningun dato o no exister mostrara un error)
            return NotFound();
        }

        /// <summary>
        /// Metodo de Insercion de Datos a la tabla tipo_equipo
        /// </summary>
        /// <param name="tipoNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/tipo_equipo")]
        public IActionResult guardarTipo([FromBody] tipo_equipo tipoNuevo)
        {
            try
            {
                ///"e" representa un alias para el listado de tipos
                IEnumerable<tipo_equipo> tipoExiste = from e in _contexto.tipo_equipo
                                                  where e.descripcion == tipoNuevo.descripcion
                                                  select e;

                ///Realiza una compracion de que sea diferente de 0 en la busqueda si existe que se realiza
                ///en la consulta y si no existe nada deja insertarlo 
                if (tipoExiste.Count() == 0)
                {
                    _contexto.tipo_equipo.Add(tipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(tipoNuevo);
                }
                return Ok(tipoExiste);
            }
            ///de no ser asi lo envia al catch y muestra un error
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de Modificacion de datos en la tabla Marca
        /// </summary>
        /// <param name="tipoAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/tipo_equipo")]
        public IActionResult updateTipo([FromBody] tipo_equipo tipoAModificar)
        {
            ///"e" representa un alias para el listado de tipos donde se comprara la seleccion para
            ///su modidicacion
            tipo_equipo tipoExiste = (from e in _contexto.tipo_equipo
                                  where e.id_tipo_equipo == tipoAModificar.id_tipo_equipo
                                  select e).FirstOrDefault();
            if (tipoExiste is null)
            {
                return NotFound();
            }

            /// Donde se identifica el valor que se esta insertando en memoria y donde se
            ///insertara en la base de datos
            tipoExiste.descripcion = tipoAModificar.descripcion;
            tipoExiste.estado = tipoAModificar.estado;

            _contexto.Entry(tipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(tipoExiste);

        }
    }
}