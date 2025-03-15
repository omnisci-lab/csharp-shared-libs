using OmniSciLab.Sql.Attributes;

namespace OmniSciLab.Sql;

public interface ISqlTable
{

}

public class SqlTableWithTimestamp : ISqlTable
{
    [SqlColumn("CreatedAt")]
    public DateTime? CreatedAt { get; set; } = null;

    [SqlColumn("UpdatedAt")] 
    public DateTime? UpdatedAt { get; set; } = null;

    [SqlColumn("DeletedAt")]
    public DateTime? DeletedAt { get; set; } = null;
}