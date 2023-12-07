using BlazorAuth.Api.Entities;

namespace BlazorAuth.Api.Repositories;
public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmail(string email);
    Task Create(Usuario usuario);
}
