using System;
using System.ComponentModel.DataAnnotations;
namespace _2017AS603.Models
{
    public class tipo_equipo
    {
        [Key]
        public int id_tipo_equipo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }      
    }
}