using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mkt.Api.Models;
using Mkt.App.Commands.Users.CreateUser;
using Mkt.App.Commands.Users.UpdateUser;
using Mkt.App.Queries.Users.GetUser;
using Mkt.Core.Entities;
using System.Threading.Tasks;

namespace Mkt.Api.Controllers;
[Route("api/users")]
//[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Query
    // api/users/id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserQuery(id);
        var user = await _mediator.Send(query);
        if (user == null)
        {
            return NotFound();
        }
        var result = Result<object>.Success(user);
        return Ok(result);
    }

    // api/users
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        var query = new GetAllUserQuery();
        var user = await _mediator.Send(query);
        var result = Result<object>.Success(user);
        return Ok(result);
    }
    #endregion

    #region Command
    // api/users
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = id }, command);
    }

    // api/users
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateUserCommand command)
    {
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = id }, command);
    }

    // api/users/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var query = new GetUserQuery(id);

        var user = await _mediator.Send(query);

        if (user == null)
        {
            return NotFound();
        }

        var result = Result<object>.Success(user);
        return Ok(result);
    }
    #endregion

}
