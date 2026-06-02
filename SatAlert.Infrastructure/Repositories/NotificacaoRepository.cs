using Microsoft.EntityFrameworkCore;
using SatAlert.Domain.Entities;
using SatAlert.Infrastructure.Data;

namespace SatAlert.Infrastructure.Repositories;

public class NotificacaoRepository(AppDbContext ctx)
{
    public IReadOnlyList<Notificacao> GetByUsuarioId(Guid usuarioId) =>
        ctx.Notificacoes.AsNoTracking()
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.DataEnvio)
            .ToList();

    public Notificacao? GetById(Guid id) =>
        ctx.Notificacoes.FirstOrDefault(n => n.Id == id);

    public bool ExistsById(Guid id) =>
        ctx.Notificacoes.FirstOrDefault(n => n.Id == id) != null;

    public Notificacao Add(Notificacao notificacao)
    {
        ctx.Notificacoes.Add(notificacao);
        ctx.SaveChanges();
        return notificacao;
    }

    public bool Update(Notificacao notificacao)
    {
        ctx.Notificacoes.Update(notificacao);
        return ctx.SaveChanges() > 0;
    }
}
