using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using ProjectTemp.Application.Aggregates.Users.Commands.CreateUser;
using ProjectTemp.Application.Aggregates.Users.Commands.DeleteUser;
using ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;
using ProjectTemp.Application.Aggregates.Users.Queries.GetUserByUsername;
using ProjectTemp.Application.Aggregates.Users.Queries.GetUserCollection;
using ProjectTemp.RestApi.V1.Aggregates.Users.Models;
using ProjectTemp.RestApi.V1.Models;

namespace ProjectTemp.RestApi.V1.Aggregates.Users.Controllers;

//TODO: Authorization must be added
[ApiVersion("1")]
[ApiController]
[Route("rest/api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseModel<UserResponse>>> CreateUser(
        [FromBody] UserRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateUserCommand>(request);
        var username = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
        var query = new GetUserByUsernameQuery { Username = username };
        var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

        return CreatedAtAction(
            nameof(GetUserByUsername),
            new { username, version = "4" },
            new ResponseModel<UserResponse> { Values = mapper.Map<UserResponse>(queryResult) });
    }

    [HttpGet]
    public async Task<ActionResult<ResponseCollectionModel<UserResponse[]>>> GetAllUsers(
        [FromQuery] SearchModel searchModel,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetUserCollectionQuery>(searchModel);
        var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

        return Ok(new ResponseCollectionModel<UserResponse>
        {
            Values = mapper.Map<UserResponse[]>(queryResult.Result),
            TotalCount = queryResult.TotalCount
        });
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<ResponseModel<UserResponse>>> GetUserByUsername(
        string username,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByUsernameQuery { Username = username };
        var queryResult = await mediator.Send(query, cancellationToken).ConfigureAwait(false);

        if (queryResult is null)
            return NotFound();

        return Ok(new ResponseModel<UserResponse> { Values = mapper.Map<UserResponse>(queryResult) });
    }

    [HttpPut("{username}")]
    public async Task<ActionResult> UpdateUser(
        string username,
        UserRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateUserCommand>(request);
        command.CurrentUsername= username;

        await mediator.Send(command, cancellationToken).ConfigureAwait(false);

        return NoContent();
    }

    [HttpDelete("{username}")]
    public async Task<ActionResult> DeleteUser(string username, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteUserCommand { Username = username }, cancellationToken)
            .ConfigureAwait(false);

        return Ok();
    }
}