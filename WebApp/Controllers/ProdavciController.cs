using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProdavciController : ApiController
    {
        [HttpGet]
        //[Route("api/prodavci")]
        public IHttpActionResult GetProizvodi(string username)
        {
            var currentUser = Baza.korisnici.Values.FirstOrDefault(u => u.KorisnickoIme == username) as Prodavac;

            // Get all products from Baza.proizvodi
            var allProducts = Baza.proizvodi.Values.ToList();

            // Filter products by the ones created by the current user
            var userProducts = allProducts.Where(p => currentUser.ObjavljeniProizvodi.Contains(p.Id));

            return Ok(userProducts);
        }
        [HttpGet]
        [Route("api/prodavci/GetProizvodi")]
        public IHttpActionResult GetProizvodi(string username, string status = null, string sortBy = null)
        {
            var currentUser = Baza.korisnici.Values.FirstOrDefault(u => u.KorisnickoIme == username) as Prodavac;

            // Get all products from Baza.proizvodi
            var allProducts = Baza.proizvodi.Values.ToList();

            // Filter products by the ones created by the current user
            var userProducts = allProducts.Where(p => currentUser.ObjavljeniProizvodi.Contains(p.Id) && !p.IsDeleted);

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "available")
                {
                    userProducts = userProducts.Where(p => p.StatusDostupnosti);
                }
                else if (status == "unavailable")
                {
                    userProducts = userProducts.Where(p => p.StatusDostupnosti);
                }
            }

            // Sort products
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "nameasc":
                        userProducts = userProducts.OrderBy(p => p.Ime);
                        break;
                    case "namedesc":
                        userProducts = userProducts.OrderByDescending(p => p.Ime);
                        break;
                    case "priceasc":
                        userProducts = userProducts.OrderBy(p => p.Cena);
                        break;
                    case "pricedesc":
                        userProducts = userProducts.OrderByDescending(p => p.Cena);
                        break;
                    case "dateasc":
                        userProducts = userProducts.OrderBy(p => p.Datum);
                        break;
                    case "datedesc":
                        userProducts = userProducts.OrderByDescending(p => p.Datum);
                        break;
                    default:
                        break;
                }
            }

            return Ok(userProducts);
        }
        [HttpPut]
        [Route("api/prodavci/UpdateProizvod/{productId}")]
        public IHttpActionResult UpdateProizvod(int productId, [FromBody] Proizvod updatedProizvod)
        {
            // Retrieve the existing product from the database
            Proizvod existingProizvod;
            if (!Baza.proizvodi.TryGetValue(productId, out existingProizvod))
            {
                return NotFound();
            }

            // Check if the product is available for modification
            if (!existingProizvod.StatusDostupnosti)
            {
                return BadRequest("The product is not available for modification.");
            }

            // Update the properties of the existing product
            existingProizvod.Ime = updatedProizvod.Ime;
            existingProizvod.Cena = updatedProizvod.Cena;
            existingProizvod.Kolicina = updatedProizvod.Kolicina;
            existingProizvod.Opis = updatedProizvod.Opis;
            existingProizvod.Slika = updatedProizvod.Slika;
            existingProizvod.Datum = updatedProizvod.Datum;
            existingProizvod.Grad = updatedProizvod.Grad;

            if(existingProizvod.Kolicina > 0)
            {
                existingProizvod.StatusDostupnosti = true;
            }
            else
            {
                existingProizvod.StatusDostupnosti = false;
            }

            return Ok();
        }

        [HttpDelete]
        [Route("api/prodavci/DeleteProizvod/{productId}")]
        public IHttpActionResult DeleteProizvod(int productId)
        {
            // Retrieve the existing product from the database
            Proizvod existingProizvod;
            if (!Baza.proizvodi.TryGetValue(productId, out existingProizvod))
            {
                return NotFound();
            }

            // Check if the product is available for deletion
            if (!existingProizvod.StatusDostupnosti)
            {
                return BadRequest("The product is not available for deletion.");
            }

            // Set the isDeleted flag to true to logically delete the product
            existingProizvod.IsDeleted = true;

            return Ok();
        }


    }

}
