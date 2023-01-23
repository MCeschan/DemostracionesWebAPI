using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApiLibros.Data;
using WebApiLibros.Models;

namespace WebApiLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly DBLibrosBootcampContext context;
        public LibroController(DBLibrosBootcampContext context)
        {
            this.context = context;
        }
        //GET—> traer todos los libros
        //GET: api/libro
        [HttpGet]
        public ActionResult<IEnumerable<Libro>> Get()
        {
            return context.Libros.ToList();

        }
        //GET—>traer todos los libros por autorId

        //GET api/libro/listado/33
        [HttpGet("listado/{id}")] //--------------RUTA PERSONALIZADA
        public ActionResult<IEnumerable<Libro>> GetAllById(int id)
        {
            List<Libro> libros = (from a in context.Libros
                                   where a.AutorId == id
                                   select a).ToList();
            return libros;

        }

        //GET→ Traer uno por Id
        //GET api/libro/5
        [HttpGet("{id}")]
        public ActionResult<Libro> GetById(int id)
        {
            Libro libro = (from a in context.Libros
                           where a.Id == id
                           select a).SingleOrDefault();
            return libro;
        }
        //POST→Insertar libros, retornar un Ok()
        //POST api/libro
        [HttpPost]
        public ActionResult Post(Libro libro)
        {
            if (!ModelState.IsValid) //si falló la validacion, entonces..
            {
                return BadRequest(ModelState);
            }
            context.Libros.Add(libro); //si está todo ok, mandamos a la base.
            context.SaveChanges();
            return Ok(); //ES CODIGO 200

        }
        //PUT→modificar libro, pasado id y modelo.retornar un NoContent()
        //PUT api/libro/2
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }
            context.Entry(libro).State = EntityState.Modified; //lo marcamos como objeto modificado
            context.SaveChanges(); //hacemos la modificación
            return NoContent();
        }

        //DELETE —>Eliminar libro. Retornar el libro eliminado
        [HttpDelete("{id}")]
        public ActionResult<Libro> Delete(int id)
        {//el libro eliminado se lo mandamos al cliente.
            var libro = (from a in context.Libros
                         where a.Id == id
                         select a).SingleOrDefault();
            //ahí buscamos al libro por id.

            if (libro == null)
            {
                return NotFound();
            }
            context.Libros.Remove(libro);
            context.SaveChanges();
            return libro;

        }
    }
}
