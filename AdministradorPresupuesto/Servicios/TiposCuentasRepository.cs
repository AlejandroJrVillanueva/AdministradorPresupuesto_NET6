using AdministradorPresupuesto.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdministradorPresupuesto.Servicios
{
    public interface ITiposCuentasRepository
    {
        Task Crear(TipoCuentaViewModel tipoCuentaViewModel);
    }
    public class TiposCuentasRepository : ITiposCuentasRepository
    {
        private readonly string connectionString;
        public TiposCuentasRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DesarrolloConnection");
        }

        public async Task Crear(TipoCuentaViewModel tipoCuentaViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                ($@"INSERT INTO TiposCuentas(Nombre,UsuarioId,Orden)
                VALUES (@Nombre,@UsuarioId,0); 
                SELECT SCOPE_IDENTITY();", tipoCuentaViewModel);

            tipoCuentaViewModel.Id = id;
        }
    }
}
