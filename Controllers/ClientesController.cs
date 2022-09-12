using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;
using PruebaTecnicaPichincha.Repositories;

namespace PruebaTecnicaPichincha.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesRepository _clientesRepository;

        public ClientesController(IClientesRepository clientesRepository)
        {
            _clientesRepository = clientesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([Bind("Id,Constrasenia,Estado")] ClienteEntity clienteEntity)
        {
            try
            {
                await _clientesRepository.CrearCliente(clienteEntity);
                return CreatedAtAction(nameof(CrearCliente), clienteEntity);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarCliente(int id, [Bind("Id,Constrasenia,Estado")] ClienteEntity clienteEntity)
        {
            if (id != clienteEntity.Id)
            {
                return NotFound();
            }
            try
            {
                await _clientesRepository.EditarCliente(id, clienteEntity);
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
                if (id == null)
                {
                    return NotFound();
                }

                await _clientesRepository.EliminarCliente(id.Value);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
