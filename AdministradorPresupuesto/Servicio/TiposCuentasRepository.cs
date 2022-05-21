using AdministradorPresupuesto.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdministradorPresupuesto.Servicio
{
    public interface ITiposCuentasRepository
    {
        void Crear(TipoCuentaViewModel tipoCuentaViewModel);
    }
    public class TiposCuentasRepository : ITiposCuentasRepository
    {
        private readonly string connectionString;
        public TiposCuentasRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DesarrolloConnection");
        }

        public void Crear(TipoCuentaViewModel tipoCuentaViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            var id = connection.QuerySingle<int>($@"INSERT INTO TiposCuentas(Nombre,UsuarioId,Orden)
                VALUES (@Nombre,@UsuarioId,0); 
                SELECT SCOPE_IDENTITY();", tipoCuentaViewModel);

            tipoCuentaViewModel.Id = id;
        }
    }
}
