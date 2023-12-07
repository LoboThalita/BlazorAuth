using BlazorAuth.Api.Entities;

namespace BlazorAuth.Api.Repositories;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetItens();
    Task<Produto> GetItem(int id);
    Task<IEnumerable<Produto>> GetItensPorCategoria(int IdCategoria);
    Task<IEnumerable<Categoria>> GetCategorias();
}

