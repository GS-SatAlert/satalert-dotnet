using Microsoft.AspNetCore.Mvc;
using SatAlert.Application.DTOs;
using SatAlert.Infrastructure.Repositories;

namespace SatAlert.API.Controllers;

/// <summary>Gerenciamento de notificações de alerta.</summary>
[ApiController]
[Route("api/[controller]")]
public class NotificacoesController(NotificacaoRepository repo, UsuarioRepository usuarioRepo) : ControllerBase
{
    /// <summary>Lista notificações de um usuário.</summary>
    [HttpGet("usuario/{usuarioId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<NotificacaoResponse>), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetByUsuario(Guid usuarioId)
    {
        if (!usuarioRepo.ExistsById(usuarioId)) return NotFound(new { erro = "Usuário não encontrado." });
        return Ok(repo.GetByUsuarioId(usuarioId).Select(NotificacaoResponse.FromDomain));
    }

    /// <summary>Registra uma nova notificação para um usuário.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(NotificacaoResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Create([FromBody] NotificacaoRequest request)
    {
        if (!usuarioRepo.ExistsById(request.UsuarioId))
            return NotFound(new { erro = "Usuário não encontrado." });

        var notificacao = repo.Add(request.ToDomain());
        return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = notificacao.UsuarioId }, NotificacaoResponse.FromDomain(notificacao));
    }

    /// <summary>Marca uma notificação como lida.</summary>
    [HttpPut("{id:guid}/marcar-lida")]
    [ProducesResponseType(typeof(NotificacaoResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult MarcarLida(Guid id)
    {
        var n = repo.GetById(id);
        if (n is null) return NotFound();
        n.MarcarComoLida();
        repo.Update(n);
        return Ok(NotificacaoResponse.FromDomain(n));
    }
}
