namespace Restaurants.Application.DTOs.Common;

public class PagedResult<T> 
{
    public PagedResult(List<T> items, int resultsCount, int pageNumber, int pageSize)
    {
        Items = items;
        ResultsCount = resultsCount;
        NumberOfPages = (int)Math.Ceiling((decimal)ResultsCount / pageSize);
        Start = (pageNumber - 1) * pageSize + 1;
        End = Start + pageSize - 1;

        if(End > ResultsCount)
            End = ResultsCount;

        if (pageNumber > NumberOfPages)
        {
            Start = (NumberOfPages - 1) * pageSize + 1;
            End = ResultsCount;
        }
    }

    public int ResultsCount { get; set; }
    public int NumberOfPages { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public IEnumerable<T> Items { get; set; }
}
