using BlazorAuth.Models.DTOs;

namespace BlazorAuth.Web.Services.Interfaces;

public interface IGerenciaProdutosLocalStorageService
{
    Task<IEnumerable<ProdutoDto>> GetCollection();
    Task RemoveCollection();
}
