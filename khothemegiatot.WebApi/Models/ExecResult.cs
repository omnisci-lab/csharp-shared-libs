using khothemegiatot.WebApi.Enums;

namespace khothemegiatot.WebApi.Models;

public class ExecResult
{
    public virtual ExecStatus Status { get; set; }

    public virtual string? Message { get; set; }
}

public class ExecResult<TModel> : ExecResult
{
    public TModel? Data { get; set; }
}