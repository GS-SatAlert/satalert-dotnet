using System.ComponentModel.DataAnnotations;
using SatAlert.Domain.Entities;

namespace SatAlert.Application.DTOs;

public class UsuarioRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(20, MinimumLength = 8)]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Região de interesse é obrigatória")]
    [StringLength(100)]
    public string RegiaoInteresse { get; set; } = string.Empty;

    public Usuario ToDomain() => new(Nome, Email, Telefone, RegiaoInteresse);
}

public record UsuarioResponse(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string RegiaoInteresse,
    bool Ativo,
    DateTime CriadoEm)
{
    public static UsuarioResponse FromDomain(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.Telefone, u.RegiaoInteresse, u.Ativo, u.CriadoEm);
}
