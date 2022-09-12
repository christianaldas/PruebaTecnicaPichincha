using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;
using PruebaTecnicaPichincha.Repositories;

namespace PruebaTecnicaPichincha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentasRepository _cuentasRepository;

        public CuentasController(ICuentasRepository cuentasRepository)
        {
            _cuentasRepository = cuentasRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([Bind("NumeroCuenta,TipoCuenta,SaldoInicial,Estado")] CuentaEntity cuentaEntity)
        {
            try
            {
                await _cuentasRepository.CrearCuenta(cuentaEntity);
                return CreatedAtAction(nameof(CrearCuenta), cuentaEntity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarCuenta(string numeroCuenta, [Bind("NumeroCuenta,TipoCuenta,SaldoInicial,Estado")] CuentaEntity cuentaEntity)
        {
            if (numeroCuenta != cuentaEntity.NumeroCuenta)
            {
                return NotFound();
            }
            try
            {
                await _cuentasRepository.EditarCuenta(numeroCuenta, cuentaEntity);
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
        public async Task<IActionResult> Delete(string? numeroCuenta)
        {
            try
            {
                if (numeroCuenta is null)
                {
                    return NotFound();
                }

                await _cuentasRepository.EliminarCuenta(numeroCuenta);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
