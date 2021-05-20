using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApplication10.Models
{
    public class ModelServices
    {
        private readonly SG_2Entities _datos = new SG_2Entities();

        public ModelServices()
        {

        }

        public IEnumerable<Persona> ObtenerPersonas()
        {
            return _datos.Persona.ToList();
        }


        private static IQueryable<Persona> queryPersonasFiltrada(string textoBuscar, int? minHijos, int? maxHijos, IQueryable<Persona> query)
        {
            if (!string.IsNullOrWhiteSpace(textoBuscar))
                query = query.Where(p => p.Nombre.Contains(textoBuscar) || p.Nombre.Contains(textoBuscar));
            if (maxHijos != null)
                query = query.Where(p => p.NumeroDeHijos <= maxHijos);
            if (minHijos != null)
                query = query.Where(p => p.NumeroDeHijos >= minHijos);
            return query;
        }

        public int ContarPersonas(string textoBuscar = null, int? minHijos = null, int? maxHijos = null)
        {
            IQueryable<Persona> query = _datos.Persona;
            query = queryPersonasFiltrada(textoBuscar, minHijos, maxHijos, query);
            return query.Count();
        }

        public IEnumerable<Persona> ObtenerPaginaDePersonasFiltrada(int paginaActual, int personasPorPagina, string columnaOrdenacion, string sentidoOrdenacion,
            string textoBuscar, int? minHijos, int? maxHijos)
        {
            // Comprobamos los datos de entrada
            sentidoOrdenacion = sentidoOrdenacion.Equals("desc", StringComparison.CurrentCultureIgnoreCase) ? sentidoOrdenacion : "asc";

            var validColumns = new[] { "apellidos", "fechanacimiento", "email", "numerodehijos" };
            if (!validColumns.Contains(columnaOrdenacion.ToLower()))
                columnaOrdenacion = "apellidos";

            if (paginaActual < 1) paginaActual = 1;
            if (personasPorPagina < 1) personasPorPagina = 10;

            // Generamos la consulta
            var query = (IQueryable<Persona>)_datos.Persona.OrderBy(m => "it." + columnaOrdenacion + " " + sentidoOrdenacion);
                

            query = queryPersonasFiltrada(textoBuscar, minHijos, maxHijos, query);

            return query
                    .Skip((paginaActual - 1) * personasPorPagina)
                    .Take(personasPorPagina)
                    .ToList();
        }


        // Alternativa usando una cadena
        public IEnumerable<Persona> ObtenerPaginaDePersonas(int paginaActual, int personasPorPagina, string criterioOrdenacion)
        {
            if (paginaActual < 1) paginaActual = 1;
            return _datos.Persona
                .OrderBy(m =>criterioOrdenacion)
                .Skip((paginaActual - 1) * personasPorPagina)
                .Take(personasPorPagina)
                .ToList();
        }
        // Alternativa usando un árbol de expresión
        public IEnumerable<Persona> ObtenerPaginaDePersonas<T>(int paginaActual, int personasPorPagina, Expression<Func<Persona, T>> ordenacion, Direccion direccion)
        {
            if (paginaActual < 1) paginaActual = 1;

            IQueryable<Persona> query = _datos.Persona;
            if (direccion == Direccion.Ascendente)
                query = query.OrderBy(ordenacion);
            else
                query = query.OrderByDescending(ordenacion);

            return query.Skip((paginaActual - 1) * personasPorPagina)
                        .Take(personasPorPagina)
                        .ToList();
        }

        public void Dispose()
        {
            _datos.Dispose();
        }
    }
}