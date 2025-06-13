using OmniSciLab.WebApi.Enums;

namespace OmniSciLab.WebApi.Models;

public class ExecResult
{
    public virtual ExecStatus Status { get; set; }

    public virtual string? Message { get; set; }

    public static ExecResult Success(string? message = null)
    {
        return new ExecResult { Status = ExecStatus.Success, Message = message };
    }

    public static ExecResult NotFound(string? message = null)
    {
        return new ExecResult { Status = ExecStatus.NotFound, Message = message };
    }

    public static ExecResult AlreadyExists(string? message = null)
    {
        return new ExecResult { Status = ExecStatus.AlreadyExists, Message = message };
    }

    public static ExecResult Invalid(string? message = null)
    {
        return new ExecResult { Status = ExecStatus.Invalid, Message = message };
    }

    public static ExecResult Failed(string? message = null)
    {
        return new ExecResult { Status = ExecStatus.Failed, Message = message };
    }

    public static ExecResult Exception(string? message = null)
    {
        return new ExecResult { Status = ExecStatus.Exception, Message = message };
    }
}

public class ExecResult<TModel> : ExecResult
{
    public TModel? Data { get; set; }

    public static ExecResult<TModel> Success(TModel data, string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Success, Data = data, Message = message };
    }

    public static new ExecResult<TModel> NotFound(string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.NotFound, Data = default!, Message = message };
    }

    public static ExecResult<TModel> NotFound(TModel data, string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.NotFound, Data = data, Message = message };
    }

    public static new ExecResult<TModel> AlreadyExists(string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.AlreadyExists, Data = default!, Message = message };
    }

    public static ExecResult<TModel> AlreadyExists(TModel data, string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.AlreadyExists, Data = data, Message = message };
    }

    public static new ExecResult<TModel> Invalid(string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Invalid, Data = default!, Message = message };
    }

    public static ExecResult<TModel> Invalid(TModel data, string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Invalid, Data = data, Message = message };
    }

    public static new ExecResult<TModel> Failed(string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Failed, Data = default!, Message = message };
    }

    public static ExecResult<TModel> Failed(TModel data, string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Failed, Data = data, Message = message };
    }

    public static new ExecResult<TModel> Exception(string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Exception, Data = default!, Message = message };
    }

    public static ExecResult<TModel> Exception(TModel data, string? message = null)
    {
        return new ExecResult<TModel> { Status = ExecStatus.Exception, Data = data, Message = message };
    }
}