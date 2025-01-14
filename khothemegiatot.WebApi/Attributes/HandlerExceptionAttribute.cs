using khothemegiatot.WebApi.Enums;
using khothemegiatot.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace khothemegiatot.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class HandlerExceptionAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;
        ObjectResult objectResult = new ObjectResult(new ExecResult
        {
            Status = ExecStatus.Exception,
            Message = exception.Message
        });

        objectResult.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.ExceptionHandled = true;
        context.Result = objectResult;
    }
}