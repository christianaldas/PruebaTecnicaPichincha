using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;
using PruebaTecnicaPichincha.Models;
using System.Configuration;
using System.Formats.Asn1;
using System.Net;

namespace PruebaTecnicaPichincha.Repositories
{
    public class MovimientosRepository : IMovimientosRepository
    {
        private readonly PruebaTecnicaPichinchaContext context;
        private readonly IConfiguration configuration;
        private decimal montoLimiteDiario;
        private decimal saldoActual = decimal.Zero;
        private decimal montoRetirado = decimal.Zero;
        public MovimientosRepository(PruebaTecnicaPichinchaContext context,
                                     decimal montoLimiteDiario)
        {
            this.context = context;
            this.montoLimiteDiario = montoLimiteDiario;
        }

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

                if (movimiento.Valor == decimal.Zero)
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

                if (configuration is not null)
                {
                    montoLimiteDiario = decimal.Parse(configuration["MontoLimiteDiario"]);
                }



                await CalcularDatosControl(movimiento,
                                           cuentaEntity);

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
                if (movimiento is null || context.Cuentas is null || context.Movimientos is null || context.Clientes is null ||
                    movimiento.CuentaNumeroCuenta is null || movimiento.Saldo != decimal.Zero)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                if (movimiento.Valor == decimal.Zero)
                {
                    throw new Exception($"Ingrese un valor a transaccionar");

                }

                CuentaEntity? cuentaEntity = await context.Cuentas.Where(c => c.NumeroCuenta != null && c.NumeroCuenta.Equals(movimiento.CuentaNumeroCuenta)).FirstOrDefaultAsync();

                if (cuentaEntity is null)
                {
                    throw new Exception($"Parece que tuvimos un error al crear el movimiento, ¿Puede revisar que el identificador de la cuenta existe?");
                }

                MovimientoEntity? movimientoEntity = await context.Movimientos.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (movimientoEntity is null)
                {
                    throw new Exception($"Parece que tuvimos un error al editar el movimiento, ¿Puede revisar que el identificador de la movimiento es correcto?");
                }

                if (configuration is not null)
                {
                    montoLimiteDiario = decimal.Parse(configuration["MontoLimiteDiario"]);
                }

                movimientoEntity.Cuenta = cuentaEntity;
                movimientoEntity.TipoMovimiento = movimiento.TipoMovimiento;
                movimientoEntity.FechaMovimiento = movimiento.FechaMovimiento;
                context.ChangeTracker.Clear();
                context.Movimientos.Update(movimientoEntity);
                
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }

        public async Task<List<ReportesModel>> DameListaMovimientoPorFecha(DateTime fecha, string numeroCuenta)
        {
            try
            {
                var reporte = await (from m in context.Movimientos
                                         join c in context.Cuentas on m.CuentaNumeroCuenta equals c.NumeroCuenta
                                         join cl in context.Clientes on c.ClienteId equals cl.Id
                                         join p in context.Personas on cl.Persona.Identificacion  equals p.Identificacion
                                         where c.NumeroCuenta.Equals(numeroCuenta) &&
                                         m.FechaMovimiento.Date == fecha.Date
                                         select new ReportesModel
                                         {
                                             Cliente = p.Nombre,
                                             Estado = c.Estado,
                                             Fecha = fecha,
                                             Movimiento = m.Valor,
                                             NumeroCuenta = c.NumeroCuenta,
                                             SaldoDisponible = m.Saldo,
                                             SaldoInicial = c.SaldoInicial,
                                             TipoCuenta = c.TipoCuenta
                                         }).ToListAsync();

                return reporte;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EliminarMovimiento(int id)
        {
            try
            {

                throw new Exception($"No puedes eliminar Movimientos Por Cuestiones de seguridad");

                if (id == decimal.Zero || context.Cuentas is null || context.Movimientos is null || context.Clientes is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }

        private async Task CalcularDatosControl(MovimientoEntity movimiento, CuentaEntity cuentaEntity)
        {
            if (context is null || context.Movimientos is null)
            {
                throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
            }
            List<MovimientoEntity>? ultimosMovimientos = await context.Movimientos.OrderByDescending(m => m.FechaMovimiento)
                                                                                     .ThenByDescending(m => m.Id)
                                                                                     .Where(m => m.CuentaNumeroCuenta != null &&
                                                                                                 m.CuentaNumeroCuenta.Equals(movimiento.CuentaNumeroCuenta))
                                                                                     .ToListAsync();



            if (ultimosMovimientos.Any())
            {
                if (movimiento.Id > decimal.Zero)
                {
                    List<MovimientoEntity> listaSinMovimiento = ultimosMovimientos.Where(ul => ul.FechaMovimiento.Date.Equals(DateTime.Now.Date) &&
                                                                                               ul.Valor < 0 &&
                                                                                               ul.Id != movimiento.Id)
                                                                                  .OrderByDescending(m => m.FechaMovimiento)
                                                                                  .ThenByDescending(m => m.Id).ToList();



                    if (listaSinMovimiento.Any())
                    {
                        montoRetirado = listaSinMovimiento.Sum(ul => ul.Valor) * -1;
                        saldoActual = listaSinMovimiento.First().Saldo;
                    }
                    else
                    {
                        saldoActual = cuentaEntity.SaldoInicial;
                    }
                }
                else
                {
                    montoRetirado = ultimosMovimientos.Where(ul => ul.FechaMovimiento.Date.Equals(DateTime.Now.Date) &&
                                                                                   ul.Valor < 0).Sum(ul => ul.Valor) * -1;
                    saldoActual = ultimosMovimientos.First().Saldo;
                }

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
        }
    }
}
