using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mkt.Api.Models;
using Mkt.App.Commands.Users.GenerateCodeReset;
using Mkt.App.Commands.Users.LoginUser;
using Mkt.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Mkt.Api.Controllers;
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    #region Command
    // api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var loginUserViewModel = await _mediator.Send(command);
        if (loginUserViewModel == null)
            return BadRequest(new { Error = "Usuário ou senha Inválidos." });
        return Ok(loginUserViewModel);
    }


    [HttpPost("generatecodereset")]
    public async Task<IActionResult> GenerateCodeReset([FromBody] GenerateCodeResetUserCommand command)
    {
        try
        {
            var userId = await _mediator.Send(command);
            var result = Result<int>.Success(userId);
            return Ok(result);
        }
        catch (UserNotFoundException ex)
        {
            _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
            var result = Result<string>.Failure("Usuário não encontrado. Verifique o e-mail fornecido.");
            return BadRequest(result);
        }
        catch (NotImplementedException ex)
        {
            _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
            var result = Result<string>.Failure("Tipo de envio não suportado.");
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.InnerException?.Message ?? ex.Message, ex);
            var result = Result<string>.Failure("Ocorreu um erro durante o processamento da solicitação.");
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }
    }

    #endregion

}
