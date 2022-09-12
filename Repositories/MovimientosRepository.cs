using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;
using System.Configuration;

namespace PruebaTecnicaPichincha.Repositories
{
    public class MovimientosRepository : IMovimientosRepository
    {
        private readonly PruebaTecnicaPichinchaContext context;
        private readonly IConfiguration configuration;

        public MovimientosRepository(PruebaTecnicaPichinchaContext context,
                                     IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task CrearMovimiento(MovimientoEntity movimiento)
        {
            try
            {
                if (movimiento is null || context.Cuentas is null || context.Movimientos is null || context.Clientes is null ||
                    movimiento.CuentaNumeroCuenta is null || movimiento.Saldo != decimal.Zero)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                if(movimiento.Valor == decimal.Zero)
                {
                    throw new Exception($"Ingrese un valor a transaccionar");

                }

                CuentaEntity? cuentaEntity = await context.Cuentas.Where(c => c.NumeroCuenta != null && c.NumeroCuenta.Equals(movimiento.CuentaNumeroCuenta)).FirstOrDefaultAsync();

                if (cuentaEntity is null)
                {
                    throw new Exception($"Parece que tuvimos un error al crear el movimiento, ¿Puede revisar que el identificador de la cuenta existe?");
                }

                MovimientoEntity? movimientoEntity = await context.Movimientos.FirstOrDefaultAsync(x => x.Id.Equals(movimiento.Id));
                if (movimientoEntity is not null)
                {
                    throw new Exception($"Parece que tuvimos un error al crear el movimiento, ¿Puede revisar que el identificador de la movimiento es única?");
                }



                int montoLimiteDiario = Int32.Parse(configuration["MontoLimiteDiario"]);
                decimal saldoActual = decimal.Zero;
                decimal montoRetirado = decimal.Zero;

                List<MovimientoEntity>? ultimosMovimientos = await context.Movimientos.OrderByDescending(m => m.FechaMovimiento)
                                                                                      .ThenByDescending(m => m.Id)
                                                                                      .Where(m => m.CuentaNumeroCuenta != null &&
                                                                                                  m.CuentaNumeroCuenta.Equals(movimiento.CuentaNumeroCuenta))
                                                                                      .ToListAsync();



                if (ultimosMovimientos.Any())
                {
                    montoRetirado = ultimosMovimientos.Where(ul => ul.FechaMovimiento.Date.Equals(DateTime.Now.Date) &&
                                                                                         ul.Valor < 0).Sum(ul => ul.Valor) * -1;
                    saldoActual = ultimosMovimientos.First().Saldo;
                }
                else
                {
                    saldoActual = cuentaEntity.SaldoInicial;
                }

                if (movimiento.Valor < 0)
                {
                    if (montoRetirado + (movimiento.Valor * -1) > montoLimiteDiario)
                    {
                        throw new Exception($"Cupo Diario Excedido");

                    }

                    if (saldoActual == decimal.Zero || saldoActual + movimiento.Valor < decimal.Zero)
                    {
                        throw new Exception($"Saldo no disponible");

                    }
                }

                movimiento.Saldo = saldoActual + movimiento.Valor;
                movimiento.Cuenta = cuentaEntity;
                movimiento.FechaMovimiento = DateTime.Now;

                EntityEntry<MovimientoEntity> movimientoCreada = await context.Movimientos.AddAsync(movimiento);

                if (movimientoCreada is null)
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

        public async Task EditarMovimiento(int id, MovimientoEntity movimiento)
        {
            try
            {
                if (movimiento is null || context.Cuentas is null || context.Movimientos is null || context.Clientes is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                if (movimiento.Cuenta is not null)
                {
                    CuentaEntity? cuentaAEditar = await context.Cuentas.Where(c => c.NumeroCuenta != null && c.NumeroCuenta.Equals(movimiento.Cuenta.NumeroCuenta)).FirstOrDefaultAsync();
                    if (cuentaAEditar is null)
                    {
                        throw new Exception($"Tuvimos un problema al actualizar el movimiento, ¿Los datos ingresados están correctos?");

                    }
                }

                MovimientoEntity? movimientoAEditar = await context.Movimientos.Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();

                if (movimientoAEditar is null)
                {
                    throw new Exception($"Tuvimos un problema al actualizar el movimiento, ¿Los datos ingresados están correctos?");
                }
                context.ChangeTracker.Clear();
                context.Movimientos.Update(movimiento);

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }

        public async Task EliminarMovimiento(int id)
        {
            try
            {
                if (context.Clientes is null || context.Movimientos is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                MovimientoEntity? movimientoAEliminar = await context.Movimientos.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (movimientoAEliminar is null)
                {
                    throw new Exception($"Tuvimos un problema al eliminar el movimiento, ¿Los datos ingresados están correctos?");
                }

                EntityEntry<MovimientoEntity> movimientoEliminado = context.Movimientos.Remove(movimientoAEliminar);
                if (movimientoEliminado is null)
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
