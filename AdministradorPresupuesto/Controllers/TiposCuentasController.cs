using AdministradorPresupuesto.Models;
using AdministradorPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AdministradorPresupuesto.Controllers
{
    public class TiposCuentasController: Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;

        public TiposCuentasController(ITiposCuentasRepository tiposCuentasRepository)
        {
            _tiposCuentasRepository = tiposCuentasRepository;   
        }

        public ITiposCuentasRepository TiposCuentasRepository { get; }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearAsync(TipoCuentaViewModel tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            
            tipoCuenta.UsuarioId = 1;

            var existeTiposCuentas = 
                await _tiposCuentasRepository.ExisteNombrePorUsuarioId(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            
            if (existeTiposCuentas)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            
            await _tiposCuentasRepository.Crear(tipoCuenta);

            return View();
        }
    }
}
