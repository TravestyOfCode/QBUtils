using MediatR;
using Microsoft.EntityFrameworkCore;

namespace QBUtils.Data.Services.SQLConnection;

public class GetSQLConnectionsByUser : IRequest<Result<List<SQLConnectionModel>>>
{
    public required string UserId { get; set; }
}

internal class GetSQLConnectionsByUserHandler : IRequestHandler<GetSQLConnectionsByUser, Result<List<SQLConnectionModel>>>
{
    private readonly AppDbContext _dbContext;

    private readonly ILogger<GetSQLConnectionsByUserHandler> _logger;

    public GetSQLConnectionsByUserHandler(AppDbContext dbContext, ILogger<GetSQLConnectionsByUserHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<List<SQLConnectionModel>>> Handle(GetSQLConnectionsByUser request, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.SQLConnections
                .Where(p => p.UserId.Equals(request.UserId))
                .ProjectToModel()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexected error handling request: {request}", request);

            return Error.ServerError();
        }
    }
}
