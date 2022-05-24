using AdministradorPresupuesto.Models;
using AdministradorPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AdministradorPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly ITiposCuentasRepository _tiposCuentasRepository;
        private readonly IServicioUsuarios _servicioUsuarios;

        public TiposCuentasController(ITiposCuentasRepository tiposCuentasRepository, IServicioUsuarios servicioUsuarios)
        {
            this._tiposCuentasRepository = tiposCuentasRepository;
            this._servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _tiposCuentasRepository.Obtener(usuarioId);
            return View(tiposCuentas);
        }

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

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _tiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuentaViewModel tipoCuentaViewModel)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var existeTipoCuenta = await _tiposCuentasRepository.ObtenerPorId(tipoCuentaViewModel.Id, usuarioId);

            if (existeTipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await _tiposCuentasRepository.Actualizar(tipoCuentaViewModel);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _tiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var existeTipoCuenta = await _tiposCuentasRepository.ObtenerPorId(id, usuarioId);

            if (existeTipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _tiposCuentasRepository.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            return Ok();
        }
    }
}
