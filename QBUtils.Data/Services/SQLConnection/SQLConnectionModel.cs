namespace QBUtils.Data;

public class SQLConnectionModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool IsDefault { get; set; }

    public required string UserId { get; set; }

    public required string QBFile { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }
}

internal static class SQLConnectionExtensions
{
    public static IQueryable<SQLConnectionModel> ProjectToModel(this IQueryable<Entity.SQLConnection> source) => source.Select(p => new SQLConnectionModel()
    {
        Id = p.Id,
        Name = p.Name,
        IsDefault = p.IsDefault,
        UserId = p.UserId,
        QBFile = p.QBFile,
        UserName = p.UserName,
        Password = p.Password
    });

    public static SQLConnectionModel AsModel(this Entity.SQLConnection entity) => new SQLConnectionModel()
    {
        Id = entity.Id,
        Name = entity.Name,
        IsDefault = entity.IsDefault,
        UserId = entity.UserId,
        QBFile = entity.QBFile,
        UserName = entity.UserName,
        Password = entity.Password
    };
}