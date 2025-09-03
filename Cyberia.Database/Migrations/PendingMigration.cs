namespace Cyberia.Database.Migrations;

/// <summary>
/// Represents a pending migration that has not yet been applied to the database.
/// </summary>
/// <param name="Name">The name of the migration.</param>
/// <param name="Order">The order of the migration in format 'yyyyMMddHHmm'.</param>
/// <param name="Script">The SQL script to apply.</param>
internal sealed record PendingMigration(string Name, long Order, string Script);
