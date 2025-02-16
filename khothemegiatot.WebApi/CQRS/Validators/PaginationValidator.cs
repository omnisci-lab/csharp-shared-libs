using FluentValidation;
using khothemegiatot.WebApi.Models;

namespace khothemegiatot.WebApi.CQRS.Validators;

public class PaginationValidator<T> : AbstractValidator<T> where T : Pagination
{
    public PaginationValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
    }
}