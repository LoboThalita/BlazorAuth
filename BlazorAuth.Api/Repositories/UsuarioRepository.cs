using BlazorAuth.Api.Entities;
using BlazorShop.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuth.Api.Repositories;


public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Create(Usuario usuario)
    {
        if (usuario is null)
            throw new ArgumentNullException(nameof(usuario));

        if (_context.Usuarios.Any(u => u.email == usuario.email))
            throw new InvalidOperationException("Email já cadastrado no sistema");

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario?> GetByEmail(string email)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == email);

        return usuario;
    }
}
