namespace OmniSciLab.Sql.QueryBuilder;

public class WhereClause
{
    public string Column { get; set; } = default!;
    public string Operator { get; set; } = default!;
    public string Value { get; set; } = default!;
}