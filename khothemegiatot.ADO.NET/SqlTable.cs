namespace khothemegiatot.ADO.NET;

public interface ISqlTable
{
    
}

public class SqlTableWithTimestamp : ISqlTable
{
    [SqlColumn("CreatedAt")]
    public DateTime? CreatedAt { get; set; }

    [SqlColumn("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [SqlColumn("DeletedAt")]
    public DateTime? DeletedAt { get; set; }
}