using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class facultadesController : Controller
    {
        private readonly _2017AS603Context _contexto;
        public facultadesController(_2017AS603Context miContexto)
        {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo Select en general de la tabla facultades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/facultades")]
        public IActionResult Get()
        {
            ///"e" representa un alias para el listado de equipos
            var facultades = from e in _contexto.facultades
                                               select e;

            ///Verifica que la lista no este vacia y retorna los datos
            if (facultades.Count() > 0)
            {
                return Ok(facultades);
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
        [Route("api/facultades/{id}")]
        public IActionResult getbyId(int id)
        {
            ///"e" representa un alias para el listado de facultades
            facultades unaFacultad = (from e in _contexto.facultades
                                where e.facultad_id == id //Filtro por ID
                                select e).FirstOrDefault();

            ///verifica que el ID preguntado tenga datos y luego los retorna
            if (unaFacultad != null)
            {
                return Ok(unaFacultad);
            }
            ///De ser nulo (No tener ningun dato o no exister mostrara un error)
            return NotFound();
        }

        /// <summary>
        /// Metodo de insersion de datos a la tabla facultades
        /// </summary>
        /// <param name="facultadNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/facultades")]
        public IActionResult guardarFacultad([FromBody] facultades facultadNuevo)
        {
            try
            {
                ///"e" representa un alias para el listado de facultades
                IEnumerable<facultades> facultadExiste = from e in _contexto.facultades
                                                    where e.nombre_facultad == facultadNuevo.nombre_facultad

                                                    select e;

                ///Realiza una compracion de que sea diferente de 0 en la busqueda si existe que se realiza
                ///en la consulta y si no existe nada deja insertarlo 
                if (facultadExiste.Count() == 0)
                {
                    _contexto.facultades.Add(facultadNuevo);
                    _contexto.SaveChanges();
                    return Ok(facultadNuevo);
                }
                return Ok(facultadExiste);
            }
            ///de no ser asi lo envia al catch y muestra un error
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Metodo de modificacion de datos de la tabla facultad
        /// </summary>
        /// <param name="facultadAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/facultades")]
        public IActionResult updateFacultad([FromBody] facultades facultadAModificar)
        {
            ///"e" representa un alias para el listado de facultades donde se comprara la seleccion para
            ///su modidicacion
            facultades facultadExiste = (from e in _contexto.facultades
                                    where e.facultad_id == facultadAModificar.facultad_id
                                    select e).FirstOrDefault();
            if (facultadExiste is null)
            {
                return NotFound();
            }

            /// Donde se identifica el valor que se esta insertando en memoria y donde se
            ///insertara en la base de datos
            facultadExiste.nombre_facultad = facultadAModificar.nombre_facultad;

            _contexto.Entry(facultadExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(facultadExiste);

        }
    }
}
