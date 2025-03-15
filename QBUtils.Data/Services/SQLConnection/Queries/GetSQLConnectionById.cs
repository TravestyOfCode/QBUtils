using MediatR;
using Microsoft.EntityFrameworkCore;
using QBUtils.Data.Services;

namespace QBUtils.Data.SQLConnection;

public class GetSQLConnectionById : IRequest<Result<SQLConnectionModel>>
{
    public int Id { get; set; }

    public required string UserId { get; set; }
}

internal class GetSQLConnectionByIdHandler : IRequestHandler<GetSQLConnectionById, Result<SQLConnectionModel>>
{
    private readonly AppDbContext _dbContext;

    private readonly ILogger<GetSQLConnectionByIdHandler> _logger;

    public GetSQLConnectionByIdHandler(AppDbContext dbContext, ILogger<GetSQLConnectionByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<SQLConnectionModel>> Handle(GetSQLConnectionById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.SQLConnections.SingleOrDefaultAsync(p => p.Id.Equals(request.Id) && p.UserId.Equals(request.UserId));

            if (entity == null)
            {
                return Error.NotFoundError();
            }

            return entity.AsModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return Error.ServerError();
        }
    }
}
