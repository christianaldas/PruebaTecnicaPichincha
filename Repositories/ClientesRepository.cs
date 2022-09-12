using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaTecnicaPichincha.Data;
using PruebaTecnicaPichincha.Entities;

namespace PruebaTecnicaPichincha.Repositories
{
    public class ClientesRepository : IClientesRepository
    {
        private readonly PruebaTecnicaPichinchaContext context;

        public ClientesRepository(PruebaTecnicaPichinchaContext context)
        {
            this.context = context;
        }
        public async Task CrearCliente(ClienteEntity cliente)
        {
            try
            {
                if (cliente is null || context.Clientes is null || context.Personas is null || cliente.Persona is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                PersonaEntity? personaEntity = await context.Personas.Where(c => c.Identificacion != null
                && c.Identificacion.Equals(cliente.Persona.Identificacion)).FirstOrDefaultAsync();

                if (personaEntity is not null)
                {
                    throw new Exception($"Parece que tuvimos un error al crear la persona, ¿Puede revisar que la identificación sea unica?");
                }

                EntityEntry<ClienteEntity> clienteCreado = await context.Clientes.AddAsync(cliente);
                if (clienteCreado is null)
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

        public async Task EditarCliente(int id, ClienteEntity cliente)
        {
            try
            {
                if (cliente is null || context.Clientes is null || context.Personas is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                if(cliente.Persona is not null)
                {
                    PersonaEntity? personaAEditar = await context.Personas.Where(c => c.Identificacion != null && c.Identificacion.Equals(cliente.Persona.Identificacion)).FirstOrDefaultAsync();
                    if(personaAEditar is null)
                    {
                        throw new Exception($"Tuvimos un problema al actualizar el cliente, ¿Los datos ingresados están correctos?");

                    }
                }

                ClienteEntity? clienteAEditar = await context.Clientes.Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();

                if (clienteAEditar is null)
                {
                    throw new Exception($"Tuvimos un problema al actualizar el cliente, ¿Los datos ingresados están correctos?");
                }
                context.ChangeTracker.Clear();
                context.Clientes.Update(cliente);

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al realizar la operación: {e.Message}");
            }
        }

        public async Task EliminarCliente(int id)
        {
            try
            {
                if (context.Clientes is null || context.Personas is null)
                {
                    throw new Exception($"Estamos experimentando errores internos, ¿Podría intentarlo más tarde?");
                }

                ClienteEntity? clienteAEliminar = await context.Clientes.Where(c => c.Id == id).FirstOrDefaultAsync();
                if (clienteAEliminar is null)
                {
                    throw new Exception($"Tuvimos un problema al eliminar el cliente, ¿Los datos ingresados están correctos?");
                }

                EntityEntry<ClienteEntity> clienteCreado = context.Clientes.Remove(clienteAEliminar);
                if (clienteCreado is null)
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
