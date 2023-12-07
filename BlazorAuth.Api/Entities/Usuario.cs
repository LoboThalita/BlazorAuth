using System.ComponentModel.DataAnnotations;

namespace BlazorAuth.Api.Entities;

public class Usuario
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    public string email { get; set; }
    public string senha { get; set; }

    public Carrinho? Carrinho { get; set; }
}

