using MediatR;
using Microsoft.EntityFrameworkCore;

namespace QBUtils.Data.Services.SQLConnection;

public class GetSQLConnectionsByUserAsDict : IRequest<Result<Dictionary<int, string>>>
{
    public required string UserId { get; set; }
}

internal class GetSQLConnectionsByUserAsDictHandler : IRequestHandler<GetSQLConnectionsByUserAsDict, Result<Dictionary<int, string>>>
{
    private readonly AppDbContext _dbContext;

    private readonly ILogger<GetSQLConnectionsByUserAsDictHandler> _logger;

    public GetSQLConnectionsByUserAsDictHandler(AppDbContext dbContext, ILogger<GetSQLConnectionsByUserAsDictHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Dictionary<int, string>>> Handle(GetSQLConnectionsByUserAsDict request, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.SQLConnections.Where(p => p.UserId.Equals(request.UserId)).ToDictionaryAsync(p => p.Id, p => p.Name, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return Error.ServerError();
        }
    }
}
