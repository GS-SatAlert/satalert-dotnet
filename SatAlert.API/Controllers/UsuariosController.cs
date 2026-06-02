using Microsoft.AspNetCore.Mvc;
using SatAlert.Application.DTOs;
using SatAlert.Infrastructure.Repositories;

namespace SatAlert.API.Controllers;

/// <summary>Gerenciamento de usuários do SatAlert.</summary>
[ApiController]
[Route("api/[controller]")]
public class UsuariosController(UsuarioRepository repo) : ControllerBase
{
    /// <summary>Lista todos os usuários.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), 200)]
    public IActionResult GetAll() =>
        Ok(repo.GetAll().Select(UsuarioResponse.FromDomain));

    /// <summary>Busca usuário por Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioResponse), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(Guid id)
    {
        var u = repo.GetById(id);
        return u is null ? NotFound() : Ok(UsuarioResponse.FromDomain(u));
    }

    /// <summary>Cria um novo usuário.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponse), 201)]
    [ProducesResponseType(400)]
    public IActionResult Create([FromBody] UsuarioRequest request)
    {
        if (repo.ExistsByEmail(request.Email))
            return BadRequest(new { erro = "E-mail já cadastrado." });

        var usuario = repo.Add(request.ToDomain());
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, UsuarioResponse.FromDomain(usuario));
    }

    /// <summary>Atualiza um usuário.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult Update(Guid id, [FromBody] UsuarioRequest request)
    {
        var usuario = repo.GetById(id);
        if (usuario is null) return NotFound();

        if (repo.ExistsByEmail(request.Email) && usuario.Email != request.Email.Trim().ToLowerInvariant())
            return BadRequest(new { erro = "E-mail já cadastrado por outro usuário." });

        usuario.Atualizar(request.Nome, request.Email, request.Telefone, request.RegiaoInteresse);
        repo.Update(usuario);
        return Ok(UsuarioResponse.FromDomain(usuario));
    }

    /// <summary>Remove um usuário e suas notificações (cascade).</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Delete(Guid id)
    {
        if (!repo.ExistsById(id)) return NotFound();
        repo.Delete(id);
        return NoContent();
    }
}
