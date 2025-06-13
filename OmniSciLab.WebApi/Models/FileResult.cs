namespace OmniSciLab.WebApi.Models;

public class FileResult : ExecResult<byte[]>
{
    public string? ContentType { get; set; }
}