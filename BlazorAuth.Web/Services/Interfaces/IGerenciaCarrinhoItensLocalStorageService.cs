using BlazorAuth.Models.DTOs;

namespace BlazorAuth.Web.Services.Interfaces;

public interface IGerenciaCarrinhoItensLocalStorageService
{
    Task<List<CarrinhoItemDto>> GetCollection();
    Task SaveCollection(List<CarrinhoItemDto> carrinhoItensDto);
    Task RemoveCollection();
}

