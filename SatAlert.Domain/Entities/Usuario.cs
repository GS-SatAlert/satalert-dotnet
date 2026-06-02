namespace SatAlert.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string RegiaoInteresse { get; private set; } = string.Empty;
    public bool Ativo { get; private set; } = true;
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    public ICollection<Notificacao> Notificacoes { get; private set; } = [];

    private Usuario() { }

    public Usuario(string nome, string email, string telefone, string regiaoInteresse)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        RegiaoInteresse = regiaoInteresse;
    }

    public void Atualizar(string nome, string email, string telefone, string regiaoInteresse)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        RegiaoInteresse = regiaoInteresse;
    }

    public void Desativar() => Ativo = false;
}
