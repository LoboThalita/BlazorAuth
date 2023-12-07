using BlazorAuth.Models.DTOs;

namespace BlazorAuth.Web.Services.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoDto>> GetItens();
    Task<ProdutoDto> GetItem(int id);

    Task<IEnumerable<CategoriaDto>> GetCategorias();
    Task<IEnumerable<ProdutoDto>> GetItensPorCategoria(int categoriaId);
}

