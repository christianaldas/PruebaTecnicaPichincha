using PruebaTecnicaPichincha.Entities;

namespace PruebaTecnicaPichincha.Repositories
{
    public interface IMovimientosRepository
    {
        Task CrearMovimiento(MovimientoEntity movimiento);
        Task EditarMovimiento(int id, MovimientoEntity movimiento);
        Task EliminarMovimiento(int id);
    }
}