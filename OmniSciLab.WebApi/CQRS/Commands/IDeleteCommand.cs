namespace OmniSciLab.WebApi.CQRS.Commands;

public interface IDeleteCommand
{
    bool ForceDelete { get; set; }
}