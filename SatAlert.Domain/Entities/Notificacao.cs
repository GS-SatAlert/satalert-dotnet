namespace SatAlert.Domain.Entities;

public class Notificacao
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UsuarioId { get; private set; }
    public string Mensagem { get; private set; } = string.Empty;
    public DateTime DataEnvio { get; private set; } = DateTime.UtcNow;
    public bool Lida { get; private set; } = false;

    public Usuario? Usuario { get; private set; }

    private Notificacao() { }

    public Notificacao(Guid usuarioId, string mensagem)
    {
        UsuarioId = usuarioId;
        Mensagem = mensagem;
    }

    public void MarcarComoLida() => Lida = true;
}
