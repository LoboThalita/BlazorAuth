using BlazorAuth.Api.Mappings;
using BlazorAuth.Api.Repositories;
using BlazorAuth.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuth.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CarrinhoCompraController : ControllerBase
{
    private readonly ICarrinhoCompraRepository carrinhoCompraRepository;
    private readonly IProdutoRepository produtoRepository;

    private ILogger<CarrinhoCompraController> logger;

    public CarrinhoCompraController(ILogger<CarrinhoCompraController> logger, IProdutoRepository produtoRepository, ICarrinhoCompraRepository carrinhoCompraRepository)
    {
        this.logger = logger;
        this.produtoRepository = produtoRepository;
        this.carrinhoCompraRepository = carrinhoCompraRepository;
    }

    [HttpGet]
    [Route("{usuarioId}/GetItens")]
    public async Task<ActionResult<IEnumerable<CarrinhoItemDto>>> GetItens(int usuarioId)
    {
        try
        {
            var carrinhoItens = await carrinhoCompraRepository.GetItens(usuarioId);
            if (carrinhoItens is null)
            {
                return NoContent();
            }

            var produtos = await this.produtoRepository.GetItens();
            if (produtos is null)
            {
                throw new Exception("Não existem produtos...");
            }

            var carrinhoItensDto = carrinhoItens.ConverterCarrinhoItensParaDto(produtos);
            return Ok(carrinhoItensDto);
        }
        catch (Exception ex)
        {
            logger.LogError("## Erro ao obter itens do carrinho");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("{Id:int}/GetItem")]
    public async Task<ActionResult<CarrinhoItemDto>> GetItem(int Id)
    {
        try
        {
            var carrinhoItem = await carrinhoCompraRepository.GetItem(Id);
            if (carrinhoItem is null)
            {
                return NotFound($"Item não encontrado");
            }

            var produto = await produtoRepository.GetItem(carrinhoItem.ProdutoId);
            if (produto is null)
            {
                return NotFound($"Item não existe na fonte de dados");
            }

            var carrinhoItemDto = carrinhoItem.ConverterCarrinhoItemParaDto(produto);
            return Ok(carrinhoItemDto);
        }
        catch (Exception ex)
        {
            logger.LogError("## Erro ao obter itens do carrinho");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CarrinhoItemDto>> PostItem([FromBody] CarrinhoItemAdicionaDto carrinhoItemAdicionaDto)
    {
        try
        {
            var novoCarrinhoItem = await carrinhoCompraRepository.AdicionaItem(carrinhoItemAdicionaDto);

            if (novoCarrinhoItem is null)
            {
                return NoContent();
            }

            var produto = await produtoRepository.GetItem(novoCarrinhoItem.ProdutoId);

            if (produto is null)
            {
                throw new Exception($"Produto não localizado (Id: {carrinhoItemAdicionaDto.ProdutoId}");
            }

            var novoCarrinhoItemDto = novoCarrinhoItem.ConverterCarrinhoItemParaDto(produto);

            return CreatedAtAction(nameof(GetItem), new { id = novoCarrinhoItemDto.Id }, novoCarrinhoItemDto);
        }
        catch (Exception ex)
        {
            logger.LogError("## Erro criar um novo item no carrinho");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<CarrinhoItemDto>> DeteleItem(int id)
    {
        try
        {
            var carrinhoItem = await carrinhoCompraRepository.DeletaItem(id);
            if (carrinhoItem is null) return NotFound();

            var produto = await produtoRepository.GetItem(carrinhoItem.ProdutoId);

            if (produto is null) return NotFound();

            var carrinhoItemDto = carrinhoItem.ConverterCarrinhoItemParaDto(produto);
            return Ok(carrinhoItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            throw;
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<CarrinhoItemDto>> AtualizaQuantidade(int id,
        CarrinhoItemAtualizaQuantidadeDto carrinhoItemAtualizaQuantidadeDto)
    {
        try
        {
            var carrinhoItem = await carrinhoCompraRepository.AtualizaQuantidade(id, carrinhoItemAtualizaQuantidadeDto);

            if (carrinhoItem == null) return NotFound();

            var produto = await produtoRepository.GetItem(carrinhoItem.ProdutoId);
            var carrinhoItemDto = carrinhoItem.ConverterCarrinhoItemParaDto(produto);
            return Ok(carrinhoItemDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

