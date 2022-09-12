using PruebaTecnicaPichincha.Entities;
using PruebaTecnicaPichincha.Models;

namespace PruebaTecnicaPichincha.Repositories
{
    public interface IMovimientosRepository
    {
        Task CrearMovimiento(MovimientoEntity movimiento);
        Task<List<ReportesModel>> DameListaMovimientoPorFecha(DateTime fecha, string numeroCuenta);
        Task EditarMovimiento(int id, MovimientoEntity movimiento);
        void EliminarMovimiento(int id);
    }
}