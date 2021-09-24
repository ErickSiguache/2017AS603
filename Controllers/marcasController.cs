using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class marcasController : ControllerBase{
        private readonly _2017AS603Context _contexto;
        public marcasController(_2017AS603Context miContexto) {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo Select en general de la tabla Marcas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/marcas")]
        public IActionResult Get(){
            ///"e" representa un alias para el listado de marcas
            IEnumerable<marcas> marcasList = from e in _contexto.marcas
                                              select e;

            ///Verifica que la lista no este vacia y retorna los datos
            if (marcasList.Count() > 0)
            {
                return Ok(marcasList);
            }
            ///De estar vacia muestra un error
            return NotFound();      
        }

        /// <summary>
        /// Metodo de Busqueda por ID en la tabla Marcas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/marcas/{id}")]
        public IActionResult getbyId(int id)
        {
            ///"e" representa un alias para el listado de marcas
            marcas unaMarca = (from e in _contexto.marcas
                                where e.id_marcas == id //Filtro por ID
                                select e).FirstOrDefault();

            ///verifica que el ID preguntado tenga datos y luego los retorna
            if (unaMarca != null)
            {
                return Ok(unaMarca);
            }
            ///De ser nulo (No tener ningun dato o no exister mostrara un error)
            return NotFound();
        }

        /// <summary>
        /// Metodo de Insercion de Datos a la tabla Marcas
        /// </summary>
        /// <param name="marcaNuevo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/marcas")]
        public IActionResult guardarMarca([FromBody] marcas marcaNuevo)
        {
            try
            {
                ///"e" representa un alias para el listado de marcas
                IEnumerable<marcas> marcaExiste = from e in _contexto.marcas
                                                    where e.nombre_marca == marcaNuevo.nombre_marca
                                                    select e;

                ///Realiza una compracion de que sea diferente de 0 en la busqueda si existe que se realiza
                ///en la consulta y si no existe nada deja insertarlo 
                if (marcaExiste.Count() == 0)
                {
                    _contexto.marcas.Add(marcaNuevo);
                    _contexto.SaveChanges();
                    return Ok(marcaNuevo);
                }
                return Ok(marcaExiste);
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
        /// <param name="marcaAModificar"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/marcas")]
        public IActionResult updateMarca([FromBody] marcas marcaAModificar)
        {
            ///"e" representa un alias para el listado de equipos donde se comprara la seleccion para
            ///su modidicacion
            marcas marcaExiste = (from e in _contexto.marcas
                                    where e.id_marcas == marcaAModificar.id_marcas
                                    select e).FirstOrDefault();
            if (marcaExiste is null)
            {
                return NotFound();
            }

            /// Donde se identifica el valor que se esta insertando en memoria y donde se
            ///insertara en la base de datos
            marcaExiste.nombre_marca = marcaAModificar.nombre_marca;
            marcaExiste.estados = marcaAModificar.estados;

            _contexto.Entry(marcaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(marcaExiste);

        }
    }
}