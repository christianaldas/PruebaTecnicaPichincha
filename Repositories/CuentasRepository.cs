using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;

namespace PruebaTecnicaPichincha.Repositories
{
    public class CuentasRepository : ICuentasRepository
    {
        private readonly PruebaTecnicaPichinchaContext context;

        public CuentasRepository(PruebaTecnicaPichinchaContext context)
        {
            this.context = context;
        }
        public async Task CrearCuenta(CuentaEntity cuenta)
        {
            try
            {
                if (cuenta is null || context.Cuentas is null || context.Clientes is null || cuenta.ClienteId == decimal.Zero)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                ClienteEntity? clienteEntity = await context.Clientes.Where(c => c.Id.Equals(cuenta.ClienteId)).FirstOrDefaultAsync();

                if (clienteEntity is null)
                {
                    throw new Exception($"Parece que tuvimos un error al crear la cuenta, ¿Puede revisar que el identificador del cliente existe?");
                }

                CuentaEntity? cuentaEntity = await context.Cuentas.FirstOrDefaultAsync(x => x.NumeroCuenta != null && x.NumeroCuenta.Equals(cuenta.NumeroCuenta));
                if (cuentaEntity is not null)
                {
                    throw new Exception($"Parece que tuvimos un error al crear la cuenta, ¿Puede revisar que el identificador de la cuenta es única?");
                }

                cuenta.Cliente = clienteEntity;

                EntityEntry<CuentaEntity> cuentaCreada = await context.Cuentas.AddAsync(cuenta);

                if (cuentaCreada is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }

        public async Task EditarCuenta(string numeroCuenta, CuentaEntity cuenta)
        {
            try
            {
                if (cuenta is null || context.Cuentas is null || context.Clientes is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                if (cuenta.Cliente is not null)
                {
                    ClienteEntity? clienteAEditar = await context.Clientes.Where(c => c.Id.Equals(cuenta.Cliente.Id)).FirstOrDefaultAsync();
                    if (clienteAEditar is null)
                    {
                        throw new Exception($"Tuvimos un problema al actualizar la cuenta, ¿Los datos ingresados están correctos?");

                    }
                }

                CuentaEntity? cuentaAEditar = await context.Cuentas.Where(c => c.NumeroCuenta != null && c.NumeroCuenta.Equals(numeroCuenta)).FirstOrDefaultAsync();

                if (cuentaAEditar is null)
                {
                    throw new Exception($"Tuvimos un problema al actualizar la cuenta, ¿Los datos ingresados están correctos?");
                }
                context.ChangeTracker.Clear();
                context.Cuentas.Update(cuenta);

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }

        public async Task EliminarCuenta(string numeroCuenta)
        {
            try
            {
                if (context.Clientes is null || context.Cuentas is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                CuentaEntity? cuentaAEliminar = await context.Cuentas.Where(c => c.NumeroCuenta == numeroCuenta).FirstOrDefaultAsync();
                if (cuentaAEliminar is null)
                {
                    throw new Exception($"Tuvimos un problema al eliminar el cuenta, ¿Los datos ingresados están correctos?");
                }

                EntityEntry<CuentaEntity> cuentaEliminada = context.Cuentas.Remove(cuentaAEliminar);
                if (cuentaEliminada is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }



    }
}
