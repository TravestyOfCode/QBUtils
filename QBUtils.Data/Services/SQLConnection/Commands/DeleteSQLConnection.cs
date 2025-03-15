using MediatR;
using Microsoft.EntityFrameworkCore;

namespace QBUtils.Data.Services.SQLConnection;

public class DeleteSQLConnection : IRequest<Result>
{
    public int Id { get; set; }

    public required string UserId { get; set; }
}

internal class DeleteSQLConnectionHandler : IRequestHandler<DeleteSQLConnection, Result>
{
    private readonly AppDbContext _dbContext;

    private readonly ILogger<DeleteSQLConnectionHandler> _logger;

    public DeleteSQLConnectionHandler(AppDbContext dbContext, ILogger<DeleteSQLConnectionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteSQLConnection request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.SQLConnections.SingleOrDefaultAsync(p => p.Id.Equals(request.Id) && p.UserId.Equals(request.UserId), cancellationToken);

            if (entity == null)
            {
                return Error.NotFoundError();
            }

            _dbContext.SQLConnections.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Result(200, "OK"); // TOOD: Add Result.OK() static method to be consistent
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return Error.ServerError();
        }
    }
}
