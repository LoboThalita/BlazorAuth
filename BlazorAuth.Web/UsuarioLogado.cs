namespace BlazorAuth.Web;

public class UsuarioLogado
{
    public UsuarioLogado(string usuarioId, int carrinhoId)
    {
        UsuarioId = usuarioId;
        CarrinhoId = carrinhoId;
    }

    public string UsuarioId { get; private set; }
    public int CarrinhoId { get; private set; }
}

