namespace khothemegiatot.WebApi.CQRS.Commands;

public interface IDeleteCommand
{
    bool ForceDelete { get; set; }
}