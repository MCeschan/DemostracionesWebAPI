using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApiPubs.Models;

namespace WebApiPubs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly pubsContext context;

        //constructor
        public StoreController(pubsContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Store>> Get()
        {
            return context.Stores.ToList();

        }
        [HttpGet("{id}")]
        public ActionResult<Store> GetById(string id)
        {
            Store store = (from a in context.Stores
                                   where a.StorId == id
                                   select a).SingleOrDefault();
            return store;
        }
        [HttpPost]
        public ActionResult Post(Store store)
        {
            if (!ModelState.IsValid) //si falló la validacion, entonces..
            {
                return BadRequest(ModelState);
            }
            context.Stores.Add(store); //si está todo ok, mandamos a la base.
            context.SaveChanges();
            return Ok(); //ES CODIGO 200

        }
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Store store)
        {
            if (id != store.StorId)
            {
                return BadRequest();
            }
            context.Entry(store).State = EntityState.Modified; //lo marcamos como objeto modificado
            context.SaveChanges(); //hacemos la modificación
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult<Store> Delete(string id)
        {
            var store = (from a in context.Stores
                             where a.StorId == id
                             select a).SingleOrDefault();
            //ahí buscamos al autor por id.

            if (store == null)
            {
                return NotFound();
            }
            context.Stores.Remove(store);
            context.SaveChanges();
            return store;

        }
        [HttpGet("{name}")]
        public ActionResult<Store> GetByName(string name)
        {
            Store store = (from a in context.Stores
                           where a.StorName == name
                           select a).SingleOrDefault();
            return store;
        }
        //GetbyZip
        [HttpGet("listado/{zip}")] //--------------RUTA PERSONALIZADA
        public ActionResult<IEnumerable<Store>> GetByZip(string zip)
        {
            List<Store> stores = (from a in context.Stores
                                  where a.Zip == zip
                                  select a).ToList();
            return stores;

        }
        //GetbyCityState
        [HttpGet("listado/{city}")] //--------------RUTA PERSONALIZADA
        public ActionResult<IEnumerable<Store>> GetByCity(string city)
        {
            List<Store> stores = (from a in context.Stores
                                   where a.City == city
                                   select a).ToList();
            return stores;

        }
    }
}
