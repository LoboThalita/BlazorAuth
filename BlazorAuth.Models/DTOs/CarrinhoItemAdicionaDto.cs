using System.ComponentModel.DataAnnotations;

namespace BlazorAuth.Models.DTOs;

public class CarrinhoItemAdicionaDto
{
    [Required]
    public int CarrinhoId { get; set; }
    [Required]
    public int ProdutoId { get; set; }
    [Required]
    public int Quantidade { get; set; }
}
