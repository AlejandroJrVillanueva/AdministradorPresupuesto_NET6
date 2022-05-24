using AdministradorPresupuesto.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdministradorPresupuesto.Servicios
{
    public interface ITiposCuentasRepository
    {
        Task Actualizar(TipoCuentaViewModel tipoCuentaViewModel);
        Task Borrar(int id);
        Task Crear(TipoCuentaViewModel tipoCuentaViewModel);
        Task<bool> ExisteNombrePorUsuarioId(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuentaViewModel>> Obtener(int usuarioId);
        Task<TipoCuentaViewModel> ObtenerPorId(int id, int usuarioId);
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
                (@"INSERT INTO TiposCuentas(Nombre,UsuarioId,Orden)
                VALUES (@Nombre,@UsuarioId,0); 
                SELECT SCOPE_IDENTITY();", tipoCuentaViewModel);

            tipoCuentaViewModel.Id = id;
        }

        public async Task<bool> ExisteNombrePorUsuarioId(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>
                (@"  SELECT 1 
                    FROM TiposCuentas
                    WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;", new { nombre, usuarioId});
            
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuentaViewModel>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuentaViewModel>(
                @"  SELECT Id, Nombre, Orden 
                    FROM TiposCuentas
                    WHERE UsuarioId = @UsuarioId;", new { usuarioId});
        }

        public async Task Actualizar(TipoCuentaViewModel tipoCuentaViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            //ExecuteAsync = "ejecuta una query que no devuelve nada"
            await connection.ExecuteAsync(
                @"  UPDATE TiposCuentas 
                    SET Nombre = @Nombre
                    WHERE Id = @Id; ", tipoCuentaViewModel);
        }
        public async Task<TipoCuentaViewModel> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuentaViewModel>(
                @"  SELECT Id, Nombre, Orden
                    FROM TiposCuentas
                    WHERE Id = @Id AND UsuarioId = @UsuarioId;", new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"  DELETE FROM TiposCuentas
                    WHERE Id = @Id;", new { id });
        }
    }
}
