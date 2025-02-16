namespace khothemegiatot.WebApi.Models;

public class PagedResult<TModel> : Pagination
{
    public List<TModel> Items { get; set; } = new List<TModel>();

    public PagedResult<TNewModel> CreatePagedResult<TNewModel>(List<TNewModel> items)
    {
        return new PagedResult<TNewModel>
        {
            Page = Page,
            PageSize = PageSize,
            TotalItems = TotalItems,
            FilterBy = FilterBy,
            FilterValue = FilterValue,
            Keyword = Keyword,
            Items = items
        };
    }
}