using AdministradorPresupuesto.Models;
using AdministradorPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AdministradorPresupuesto.Controllers
{
    public class TiposCuentasController: Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;
        private readonly IServicioUsuarios _servicioUsuarios;

        public TiposCuentasController(ITiposCuentasRepository tiposCuentasRepository, IServicioUsuarios servicioUsuarios)
        {
            this._tiposCuentasRepository = tiposCuentasRepository;
            this._servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _tiposCuentasRepository.Obtener(usuarioId);
            return View(tiposCuentas);
        }
        
        [HttpPost]
        public async Task<IActionResult> CrearAsync(TipoCuentaViewModel tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var existeTiposCuentas = 
                await _tiposCuentasRepository.ExisteNombrePorUsuarioId(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            
            if (existeTiposCuentas)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            
            await _tiposCuentasRepository.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ValidarExisteTiposCuentas(string nombre)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var existeTiposCuentas = await _tiposCuentasRepository.ExisteNombrePorUsuarioId(nombre, usuarioId);
            if (existeTiposCuentas)
            {
                return Json($"El nombre {nombre} ya existe");
            }
            return Json("true");
        }
    }
}
