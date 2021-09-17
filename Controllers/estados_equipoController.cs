using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;

namespace _2017AS603.Controllers
{
    [ApiController]
    public class estados_equipoController : ControllerBase{
        private readonly _2017AS603Context _contexto;
        public estados_equipoController(_2017AS603Context miContexto) {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/estados_equipo")]
        public IActionResult Get(){
            var estados_equipoList = _contexto.estados_equipo;
            return Ok(estados_equipoList);         
        } 
    }
}