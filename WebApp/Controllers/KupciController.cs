using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;
using WebApp.Models.Bindings;

namespace WebApp.Controllers
{
    public class KupciController : ApiController
    {
        // GET api/profile
        //  [Authorize]
        public IHttpActionResult Get(string username)
        {
            // Retrieve the user information from the JWT token's claims
            // string username = Request.Headers.GetValues("Username").FirstOrDefault();

            // Retrieve the user profile from your existing database using Baza.korisnici.Values.ToList()
            List<Korisnik> korisnici = Baza.korisnici.Values.ToList();
            Korisnik korisnik = korisnici.FirstOrDefault(u => u.KorisnickoIme == username);
            if (korisnik == null)
            {
                return NotFound();
            }

            return Ok(korisnik);
        }

        // PUT api/profile
        //[Authorize]
        public IHttpActionResult Put(UpdateProfilBindingModel updateKorisnika)
        {
            // Retrieve the user information from the JWT token's claims
            List<Korisnik> korisnici = Baza.korisnici.Values.ToList();
            Korisnik korisnik = korisnici.FirstOrDefault(u => u.KorisnickoIme == updateKorisnika.KorisnickoIme);
            if (korisnik == null)
            {
                return NotFound();
            }

            korisnik.Ime = !string.IsNullOrEmpty(updateKorisnika.Ime) ? updateKorisnika.Ime : korisnik.Ime;
            korisnik.Prezime = !string.IsNullOrEmpty(updateKorisnika.Prezime) ? updateKorisnika.Prezime : korisnik.Prezime;
            korisnik.Lozinka = !string.IsNullOrEmpty(updateKorisnika.Lozinka) ? updateKorisnika.Lozinka : korisnik.Lozinka;
            korisnik.Email = !string.IsNullOrEmpty(updateKorisnika.Email) ? updateKorisnika.Email : korisnik.Email;
            DateTime parsedDatum;
            if (DateTime.TryParse(updateKorisnika.DatumRodjenja, out parsedDatum))
            {
                korisnik.DatumRodjenja = parsedDatum;
            }

            // korisnik.DatumRodjenja = DateTime.Parse(updateKorisnika.DatumRodjenja  ?? updateKorisnika.DatumRodjenja);

            return Ok();
        }




    }
}
