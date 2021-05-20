using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class HomeController : Controller
    {
        const int PERSONAS_POR_PAGINA = 30;
        private ModelServices _services = new ModelServices();
        public ActionResult Index(int page = 1, string sort = "Apellidos", string sortDir = "ASC",
                                   string buscar = null, int? minHijos = null, int? maxHijos = null)
        {
            var numPersonas = _services.ContarPersonas(buscar, minHijos, maxHijos);
            var personas = _services.ObtenerPaginaDePersonasFiltrada(page, PERSONAS_POR_PAGINA, sort, sortDir,
                                                                        buscar, minHijos, maxHijos);

            var datos = new PaginaDePersonasViewModel()
            {
                NumeroDePersonas = numPersonas,
                PersonasPorPagina = PERSONAS_POR_PAGINA,
                Personas = personas
            };

            return View(datos);
        }

            public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}