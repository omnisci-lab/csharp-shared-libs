using FluentValidation;
using OmniSciLab.WebApi.Models;

namespace OmniSciLab.WebApi.CQRS.Validators;

public class PaginationValidator<T> : AbstractValidator<T> where T : Pagination
{
    public PaginationValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
    }
}