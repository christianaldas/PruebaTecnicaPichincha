using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaPichincha.Models;
using PruebaTecnicaPichincha.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebaTecnicaPichincha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IMovimientosRepository movimientosRepository;

        public ReportesController(IMovimientosRepository movimientosRepository)
        {
            this.movimientosRepository = movimientosRepository;
        }
        [HttpGet("{fechaConsulta}/{numeroCuenta}")]
        public async Task<ActionResult<List<ReportesModel>>> DameListaMovimientoPorFecha(DateTime fechaConsulta, string numeroCuenta)
        {
            try
            {
                return Ok(await movimientosRepository.DameListaMovimientoPorFecha(fechaConsulta, numeroCuenta));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
