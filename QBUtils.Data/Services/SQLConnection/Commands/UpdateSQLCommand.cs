using MediatR;
using Microsoft.EntityFrameworkCore;

namespace QBUtils.Data.Services.SQLConnection;

public class UpdateSQLCommand : IRequest<Result<SQLConnectionModel>>
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool IsDefault { get; set; }

    public required string UserId { get; set; }

    public required string QBFile { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }
}

internal class UpdateSQLCommandHandler : IRequestHandler<UpdateSQLCommand, Result<SQLConnectionModel>>
{
    private readonly AppDbContext _dbContext;

    private readonly ILogger<UpdateSQLCommandHandler> _logger;

    public UpdateSQLCommandHandler(AppDbContext dbContext, ILogger<UpdateSQLCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<SQLConnectionModel>> Handle(UpdateSQLCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.SQLConnections.SingleOrDefaultAsync(p => p.Id.Equals(request.Id) && p.UserId.Equals(request.UserId), cancellationToken);

            if (entity == null)
            {
                return Error.NotFoundError();
            }

            // Check if we have any other defaults if we are setting this to be the default
            if (request.IsDefault)
            {
                var defaultConn = await _dbContext.SQLConnections.SingleOrDefaultAsync(p => p.UserId.Equals(request.UserId) && p.IsDefault && !p.Id.Equals(request.Id), cancellationToken);

                if (defaultConn != null)
                {
                    defaultConn.IsDefault = false;
                }
            }

            entity.Name = request.Name;
            entity.IsDefault = request.IsDefault;
            entity.QBFile = request.QBFile;
            entity.UserName = request.UserName;
            entity.Password = request.Password;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.AsModel();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return Error.ServerError();
        }
    }
}
