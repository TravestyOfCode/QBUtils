using MediatR;
using Microsoft.EntityFrameworkCore;
using QBUtils.Data.Services;

namespace QBUtils.Data.SQLConnection;

public class CreateSQLConnection : IRequest<Result<SQLConnectionModel>>
{
    public required string Name { get; set; }

    public bool IsDefault { get; set; }

    public required string UserId { get; set; }

    public required string QBFile { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }
}

internal class CreateSQLConnectionHandler : IRequestHandler<CreateSQLConnection, Result<SQLConnectionModel>>
{
    private readonly AppDbContext _dbContext;

    private readonly ILogger<CreateSQLConnectionHandler> _logger;

    public CreateSQLConnectionHandler(AppDbContext dbContext, ILogger<CreateSQLConnectionHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<SQLConnectionModel>> Handle(CreateSQLConnection request, CancellationToken cancellationToken)
    {
        try
        {
            // Check for any existing default for the user if we have a new default
            if (request.IsDefault)
            {
                var defaultConn = await _dbContext.SQLConnections.SingleOrDefaultAsync(p => p.UserId.Equals(request.UserId) && p.IsDefault, cancellationToken);
                if (defaultConn != null)
                {
                    defaultConn.IsDefault = false;
                }
            }

            var entity = _dbContext.SQLConnections.Add(new Entity.SQLConnection()
            {
                Name = request.Name,
                IsDefault = request.IsDefault,
                UserId = request.UserId,
                QBFile = request.QBFile,
                UserName = request.UserName,
                Password = request.Password
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Entity.AsModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return Error.ServerError();
        }
    }
}
