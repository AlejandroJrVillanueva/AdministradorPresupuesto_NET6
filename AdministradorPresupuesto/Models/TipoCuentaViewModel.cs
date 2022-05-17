using AdministradorPresupuesto.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace AdministradorPresupuesto.Models
{
    public class TipoCuentaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 4,
            ErrorMessage = "La longitud del campo {0} debe estar entre {2} y {1}")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

    }
}
