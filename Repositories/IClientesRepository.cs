using PruebaTecnicaPichincha.Entities;

namespace PruebaTecnicaPichincha.Repositories
{
    public interface IClientesRepository
    {
        Task CrearCliente(ClienteEntity cliente);
        Task EditarCliente(int id, ClienteEntity cliente);
        Task EliminarCliente(int id);
    }
}