using BlazorAuth.Api.Entities;
using BlazorAuth.Models.DTOs;

namespace BlazorAuth.Api.Repositories;

public interface ICarrinhoCompraRepository
{
    Task<CarrinhoItem> AdicionaItem(CarrinhoItemAdicionaDto carrinhoItemAdicionaDto);
    Task<CarrinhoItem> AtualizaQuantidade(int id, CarrinhoItemAtualizaQuantidadeDto carrinhoItemAtualizaQuantidadeDto);
    Task<CarrinhoItem> DeletaItem(int id);
    Task<CarrinhoItem> GetItem(int id);
    Task<IEnumerable<CarrinhoItem>> GetItens(int usuarioId);
}
