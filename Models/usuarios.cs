using System;
using System.ComponentModel.DataAnnotations;

namespace _2017AS603.Models
{
    public class usuarios
    {
        [Key]
        public int usuario_id { get; set; }
        public string nombre { get; set; }
        public string documento { get; set; }
        public char tipo { get; set; }
        public string carnet { get; set; }
        public int carrera_id { get; set; }
    }
}
