using Microsoft.AspNetCore.Mvc;
using NetCoreLinqSQL.Data;
using NetCoreLinqSQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreLinqSQL.Controllers
{
    public class EnfermosController : Controller
    {
        EnfermosContext context;

        public EnfermosController()
        {
            this.context = new EnfermosContext();
        }

        public IActionResult EliminarEnfermo()
        {
            List<Enfermo> enfermos = this.context.GetEnfermos();
            return View(enfermos);
        }

        [HttpPost]
        public IActionResult EliminarEnfermo(int inscripcion)
        {
            this.context.EliminarEnfermo(inscripcion);
            List<Enfermo> enfermos = this.context.GetEnfermos();
            return View(enfermos);
        }
    }
}
