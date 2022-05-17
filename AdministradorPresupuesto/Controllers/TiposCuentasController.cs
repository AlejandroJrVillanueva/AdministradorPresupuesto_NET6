using AdministradorPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdministradorPresupuesto.Controllers
{
    public class TiposCuentasController: Controller
    {
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(TipoCuentaViewModel tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            return View();
        }
    }
}
