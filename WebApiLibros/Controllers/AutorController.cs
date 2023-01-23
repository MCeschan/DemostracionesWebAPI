using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebApiLibros.Data;
using WebApiLibros.Models;

namespace WebApiLibros.Controllers
{
    // api/autor
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        //ESTO ES INYECCIÓN DE DEPENDENCIA---INICIA
        //propiedad
        private readonly DBLibrosBootcampContext context;

        //constructor
        public AutorController(DBLibrosBootcampContext context)
        {
            this.context = context;
        }
        //FINALIZA

        //GET: api/autor
        [HttpGet]
        public ActionResult<IEnumerable<Autor>> Get()
        {
            return context.Autores.ToList();

        }
        //GET api/autor/5
        [HttpGet("{id}")]
        public ActionResult<Autor> GetById(int id)
        {
            Autor autor = (from a in context.Autores
                           where a.IdAutor == id
                           select a).SingleOrDefault();
            return autor;
        }

        //GET api/autor/listado/33
        [HttpGet("listado/{edad}")] //--------------RUTA PERSONALIZADA
        public ActionResult<IEnumerable<Autor>> GetEdad(int edad)
        {
           List<Autor> autores = (from a in context.Autores
                                    where a.Edad==edad
                                    select a).ToList();
            return autores;

        }

        //--------INSERT
        //POST api/autor
        [HttpPost]
        public ActionResult Post(Autor autor)
        {
            if (!ModelState.IsValid) //si falló la validacion, entonces..
            {
                return BadRequest(ModelState);
            }
            context.Autores.Add(autor); //si está todo ok, mandamos a la base.
            context.SaveChanges();
            return Ok(); //ES CODIGO 200

        }
        //------UPDATE
        //PUT api/autor/2
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor autor)
        {
            if (id != autor.IdAutor)
            {
                return BadRequest();
            }
            context.Entry(autor).State = EntityState.Modified; //lo marcamos como objeto modificado
            context.SaveChanges(); //hacemos la modificación
            return Ok();
        }

        //-------DELETE
        //DELETE api/autor/1
        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id) 
        {//el autor eliminado se lo mandamos al cliente.
            var autor = (from a in context.Autores
                         where a.IdAutor == id
                         select a).SingleOrDefault();
            //ahí buscamos al autor por id.

            if(autor == null)
            {
                return NotFound();
            }
            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;

        }
    }
}
