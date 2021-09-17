using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class tipo_equipoController : ControllerBase{
        private readonly _2017AS603Context _contexto;
        public tipo_equipoController(_2017AS603Context miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/tipo_equipo")]
        public IActionResult Get(){
            var tipo_equipoList = _contexto.tipo_equipo;
            return Ok(tipo_equipoList);         
        } 
    }
}