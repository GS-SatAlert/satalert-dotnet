using System.ComponentModel.DataAnnotations;
using SatAlert.Domain.Entities;

namespace SatAlert.Application.DTOs;

public class NotificacaoRequest
{
    [Required(ErrorMessage = "UsuarioId é obrigatório")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "Mensagem é obrigatória")]
    [StringLength(1000, MinimumLength = 1)]
    public string Mensagem { get; set; } = string.Empty;

    public Notificacao ToDomain() => new(UsuarioId, Mensagem);
}

public record NotificacaoResponse(
    Guid Id,
    Guid UsuarioId,
    string Mensagem,
    DateTime DataEnvio,
    bool Lida)
{
    public static NotificacaoResponse FromDomain(Notificacao n) =>
        new(n.Id, n.UsuarioId, n.Mensagem, n.DataEnvio, n.Lida);
}
