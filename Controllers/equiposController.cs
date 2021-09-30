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

        /// <summary>
        /// Metodo Select en general de la tabla equipos
        /// </summary>
        /// <returns></returns>
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
        /// Metodo Select en general de la tabla equipos con inner join
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equiposInner")]
        public IActionResult equiposInner()
        {
            ///"e" representa un alias para el listado de equipos y "tip" el listado de tipos de equipo
            /// "mar" las marcas y "est" los estados
            var equiposListInner = from e in _contexto.equipos
                                   join tip in _contexto.tipo_equipo on e.tipo_equipo_id equals tip.id_tipo_equipo
                                   join mar in _contexto.marcas on e.marca_id equals mar.id_marcas
                                   join est in _contexto.estados_equipo on e.estado_equipo_id equals est.id_estados_equipo
                                               select new {
                                                   e.id_equipos,
                                                   e.nombre,
                                                   e.descripcion,
                                                   tip_equipo_des = tip.descripcion, ///No pueden a ver dos propiedades
                                                   mar.nombre_marca,                ///con el mismos nombre
                                                   e.modelo,
                                                   e.anio_compra,
                                                   e.costo,
                                                   e.vida_util,
                                                   estado_equipo_des = est.estado, ///Por ello se coloca un alias 
                                                   e.estado                        ///para no confundir al igual que en SQL Server
                                               };

            ///Verifica que la lista no este vacia y retorna los datos
            if (equiposListInner.Count() > 0)
            {
                return Ok(equiposListInner);
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
        [Route("api/equipos/buscarnombre/{buscarNombre}")]
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



        /// <summary>
        /// Metodo de retorno de registros de equipos filtrado por el valor dado en el parametro
        /// </summary>
        /// <param name="buscarEstado"> Representa el valor dado en el parametro en el campo 
        /// Descripcion de la tabla estado_marca </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipos/buscarequiestado/{buscarEstado}")]
        public IActionResult equiposEstado(string buscarEstado)
        {
            //"e" representa un alias para el listado de equipos
            IEnumerable<equipos> equipoPorEstado = from e in _contexto.equipos 
                                                   join d in _contexto.estados_equipo 
                                                   on e.estado_equipo_id equals d.id_estados_equipo
                                                   where d.descripcion.Contains(buscarEstado)
                                                   select e;
            ///Se realiza el if para identificar que si encontro un dato y lo retorma mostrando esos datos
            if (equipoPorEstado.Count() > 0)
            {
                return Ok(equipoPorEstado);
            }
            ///De ser menor a 0 retornara un error
            return NotFound();
        }
    }
}