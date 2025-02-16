namespace khothemegiatot.WebApi.Models;

public class Pagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get => (int)Math.Ceiling(TotalItems / (double)PageSize); }
    public int TotalItems { get; set; }
    public string? FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public string? Keyword { get; set; }
}