using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2017AS603.Models;

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
            var equiposList = _contexto.equipos;
            return Ok(equiposList);         
        } 
    }
}