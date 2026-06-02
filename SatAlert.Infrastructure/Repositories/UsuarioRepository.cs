using Microsoft.EntityFrameworkCore;
using SatAlert.Domain.Entities;
using SatAlert.Infrastructure.Data;

namespace SatAlert.Infrastructure.Repositories;

public class UsuarioRepository(AppDbContext ctx)
{
    public IReadOnlyList<Usuario> GetAll() =>
        ctx.Usuarios.AsNoTracking().OrderBy(u => u.Nome).ToList();

    public Usuario? GetById(Guid id) =>
        ctx.Usuarios.Include(u => u.Notificacoes).FirstOrDefault(u => u.Id == id);

    public bool ExistsByEmail(string email) =>
        ctx.Usuarios.Count(u => u.Email == email.Trim().ToLowerInvariant()) > 0;

    public bool ExistsById(Guid id) =>
        ctx.Usuarios.FirstOrDefault(u => u.Id == id) != null;

    public Usuario Add(Usuario usuario)
    {
        ctx.Usuarios.Add(usuario);
        ctx.SaveChanges();
        return usuario;
    }

    public bool Update(Usuario usuario)
    {
        ctx.Usuarios.Update(usuario);
        return ctx.SaveChanges() > 0;
    }

    public bool Delete(Guid id)
    {
        var u = ctx.Usuarios.Find(id);
        if (u is null) return false;
        ctx.Usuarios.Remove(u);
        ctx.SaveChanges();
        return true;
    }
}
