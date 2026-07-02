using System.Data.Common;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DiplomaManagementSystem.Infrastructure.Persistence;

internal sealed class PostgresSearchPathInterceptor : DbConnectionInterceptor
{
    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        SetSearchPath(connection);
        base.ConnectionOpened(connection, eventData);
    }

    public override async Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        await SetSearchPathAsync(connection, cancellationToken);
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    private static void SetSearchPath(DbConnection connection)
    {
        using DbCommand command = connection.CreateCommand();
        command.CommandText = "SET search_path TO public";
        command.ExecuteNonQuery();
    }

    private static async Task SetSearchPathAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using DbCommand command = connection.CreateCommand();
        command.CommandText = "SET search_path TO public";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
