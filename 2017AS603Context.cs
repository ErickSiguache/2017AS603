using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using _2017AS603.Models;

namespace _2017AS603
{
    public class _2017AS603Context : DbContext
    {
        public _2017AS603Context(DbContextOptions<_2017AS603Context> options) : base(options)
        {            
        }

        //Contexto de los metodos
        public DbSet<equipos> equipos {get; set;}
        public DbSet<estados_equipo> estados_equipo {get; set;}
        public DbSet<marcas> marcas {get; set;}
        public DbSet<tipo_equipo> tipo_equipo {get; set;}
        public DbSet<facultades> facultades { get; set; }
        public DbSet<carreras> carreras { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<reservas> reservas { get; set; }
        public DbSet<estados_reserva> estados_reserva { get; set; } 
    }
}
