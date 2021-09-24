using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class equiposController : ControllerBase{
        private readonly _2017AS603Context _contexto;
        public equiposController(_2017AS603Context miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/equipos")]
        public IActionResult Get(){
            ///"e" representa un alias para el listado de equipos
            IEnumerable<equipos> equiposList = from e in _contexto.equipos
                                               select e;

            ///Verifica que la lista no este vacia y retorna los datos
            if (equiposList.Count() > 0)
            {
                return Ok(equiposList);
            }
            ///De estar vacia muestra un error
            return NotFound();         
        }

        /// <summary>
        /// Metodo de retorno de registros filtras por ID
        /// </summary>
        /// <param name="id"> Representa el valor entero del campo ID </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipos/{id}")]
        public IActionResult getbyId(int id)
        {
            ///"e" representa un alias para el listado de equipos
            equipos unEquipo = (from e in _contexto.equipos
                                where e.id_equipos == id //Filtro por ID
                                select e).FirstOrDefault();

            ///verifica que el ID preguntado tenga datos y luego los retorna
            if (unEquipo != null)
            {
                return Ok(unEquipo);
            }
            ///De ser nulo (No tener ningun dato o no exister mostrara un error)
            return NotFound();
        }

        /// <summary>
        /// Metodo de retorno de registros de equipos filtrado por el valor dado en el parametro
        /// </summary>
        /// <param name="buscarNombre"> Representa el valor dado en el parametro en el campo Nombre </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipo/buscarnombre/{buscarNombre}")]
        public IActionResult obtenerNombre(string buscarNombre)
        {
            //"e" representa un alias para el listado de equipos
            IEnumerable<equipos> equipoPorNombre = from e in _contexto.equipos
                                                   where e.nombre.Contains(buscarNombre)
                                                   select e;
            ///Se realiza el if para identificar que si encontro un dato y lo retorma mostrando esos datos
            if (equipoPorNombre.Count() > 0)
            {
                return Ok(equipoPorNombre);
            }
            ///De ser menor a 0 retornara un error
            return NotFound();
        }

        /// <summary>
        /// Metodo de insersion de datos a la tabla equipos
        /// </summary>
        /// <param name="equipoNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/equipos")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo)
        {
            try
            {
                ///"e" representa un alias para el listado de equipos
                IEnumerable<equipos> equipoExiste = from e in _contexto.equipos
                                                    where e.nombre == equipoNuevo.nombre

                                                    select e;

                ///Realiza una compracion de que sea diferente de 0 en la busqueda si existe que se realiza
                ///en la consulta y si no existe nada deja insertarlo 
                if (equipoExiste.Count() == 0)
                {
                    _contexto.equipos.Add(equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(equipoNuevo);
                }
                return Ok(equipoExiste);
            }
            ///de no ser asi lo envia al catch y muestra un error
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de modificacion de datos de la tabla equipos
        /// </summary>
        /// <param name="equipoAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody] equipos equipoAModificar)
        {
            ///"e" representa un alias para el listado de equipos donde se comprara la seleccion para
            ///su modidicacion
            equipos equipoExiste = (from e in _contexto.equipos
                                    where e.id_equipos == equipoAModificar.id_equipos
                                    select e).FirstOrDefault();
            if (equipoExiste is null)
            {
                return NotFound();
            }

            /// Donde se identifica el valor que se esta insertando en memoria y donde se
            ///insertara en la base de datos
            equipoExiste.nombre = equipoAModificar.nombre;
            equipoExiste.descripcion = equipoAModificar.descripcion;
            equipoExiste.modelo = equipoAModificar.modelo;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            
            return Ok(equipoExiste);

        }
    }
}