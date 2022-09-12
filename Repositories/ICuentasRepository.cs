using PruebaTecnicaPichincha.Entities;

namespace PruebaTecnicaPichincha.Repositories
{
    public interface ICuentasRepository
    {
        Task CrearCuenta(CuentaEntity cuenta);
        Task EditarCuenta(string numeroCuenta, CuentaEntity cuenta);
        Task EliminarCuenta(string numeroCuenta);
    }
}