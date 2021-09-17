using System;
using System.ComponentModel.DataAnnotations;
namespace _2017AS603.Models
{
    public class equipos
    {
        [Key]
        public int id_equipos { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int tipo_equipo_id { get; set; }
        public int marca_id { get; set; }
        public string modelo { get; set; }
        public int anio_compra { get; set; }
        public decimal costo { get; set; }
        public int vida_util { get; set; }
        public int estado_equipo_id { get; set; }
        public string estado { get; set; }        
    }
}