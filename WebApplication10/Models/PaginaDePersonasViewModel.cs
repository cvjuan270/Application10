using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication10.Models
{
    public class PaginaDePersonasViewModel
    {
        public int NumeroDePersonas { get; set; }
        public IEnumerable<Persona> Personas { get; set; }
        public int PersonasPorPagina { get; set; }
    }
}