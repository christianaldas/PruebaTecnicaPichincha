using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;
using PruebaTecnicaPichincha.Repositories;

namespace PruebaTecnicaPichincha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientosRepository _cuentasRepository;

        public MovimientosController(IMovimientosRepository cuentasRepository)
        {
            _cuentasRepository = cuentasRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CrearMovimiento([Bind("Id,NumeroCuenta,FechaMovimiento,TipoMovimiento,Valor,Saldo")] MovimientoEntity cuentaEntity)
        {
            try
            {
                await _cuentasRepository.CrearMovimiento(cuentaEntity);
                return CreatedAtAction(nameof(CrearMovimiento), cuentaEntity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarMovimiento(int id, [Bind("Id,NumeroCuenta,FechaMovimiento,TipoMovimiento,Valor,Saldo")] MovimientoEntity cuentaEntity)
        {
            if (id != cuentaEntity.Id)
            {
                return NotFound();
            }
            try
            {
                await _cuentasRepository.EditarMovimiento(id, cuentaEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id is null)
                {
                    return NotFound();
                }

                await _cuentasRepository.EliminarMovimiento(id.Value);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
