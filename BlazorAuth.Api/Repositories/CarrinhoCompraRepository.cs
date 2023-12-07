using BlazorAuth.Api.Entities;
using BlazorAuth.Models.DTOs;
using BlazorShop.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuth.Api.Repositories;


public class CarrinhoCompraRepository : ICarrinhoCompraRepository
{
    private readonly AppDbContext _appDbContext;

    public CarrinhoCompraRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<CarrinhoItem> AdicionaItem(CarrinhoItemAdicionaDto carrinhoItemAdicionaDto)
    {
        if (await CarrinhoItemJaExiste(carrinhoItemAdicionaDto.CarrinhoId, carrinhoItemAdicionaDto.ProdutoId) == false)
        {
            var item = await (from produto in _appDbContext.Produtos
                              where produto.Id == carrinhoItemAdicionaDto.ProdutoId
                              select new CarrinhoItem
                              {
                                  CarrinhoId = carrinhoItemAdicionaDto.CarrinhoId,
                                  ProdutoId = produto.Id,
                                  Quantidade = carrinhoItemAdicionaDto.Quantidade
                              }).SingleOrDefaultAsync();

            if (item is not null)
            {
                var resultado = await _appDbContext.CarrinhoItens.AddAsync(item);
                await _appDbContext.SaveChangesAsync();
                return resultado.Entity;
            }
        }
        return null;
    }

    public async Task<CarrinhoItem> AtualizaQuantidade(int id, CarrinhoItemAtualizaQuantidadeDto carrinhoItemAtualizaQuantidadeDto)
    {
        var carrinhoItem = await _appDbContext.CarrinhoItens.FindAsync(id);

        if (carrinhoItem is not null)
        {
            carrinhoItem.Quantidade = carrinhoItemAtualizaQuantidadeDto.Quantidade;

            await _appDbContext.SaveChangesAsync();
            return carrinhoItem;
        }
        return null;
    }

    public async Task<CarrinhoItem> DeletaItem(int id)
    {
        var item = await _appDbContext.CarrinhoItens.FindAsync(id);

        if (item is not null)
        {
            _appDbContext.CarrinhoItens.Remove(item);
            await _appDbContext.SaveChangesAsync();
        }
        return item;
    }

    public async Task<CarrinhoItem?> GetItem(int id)
    {
        return await (from carrinho in _appDbContext.Carrinhos
                      join carrinhoItem in _appDbContext.CarrinhoItens
                      on carrinho.Id equals carrinhoItem.CarrinhoId
                      where carrinho.UsuarioId == id
                      select new CarrinhoItem
                      {
                          Id = carrinhoItem.Id,
                          ProdutoId = carrinhoItem.ProdutoId,
                          Quantidade = carrinhoItem.Quantidade,
                          CarrinhoId = carrinhoItem.CarrinhoId
                      }).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CarrinhoItem>> GetItens(int usuarioId)
    {
        return await (from carrinho in _appDbContext.Carrinhos
                      join carrinhoItem in _appDbContext.CarrinhoItens
                      on carrinho.Id equals carrinhoItem.CarrinhoId
                      where carrinho.UsuarioId == usuarioId
                      select new CarrinhoItem
                      {
                          Id = carrinhoItem.Id,
                          ProdutoId = carrinhoItem.ProdutoId,
                          Quantidade = carrinhoItem.Quantidade,
                          CarrinhoId = carrinhoItem.CarrinhoId
                      }).ToListAsync();
    }

    private async Task<bool> CarrinhoItemJaExiste(int carrinhoId, int ProdutoId)
    {
        return await _appDbContext.CarrinhoItens.AnyAsync(c => c.CarrinhoId == carrinhoId && c.ProdutoId == ProdutoId);
    }
}
