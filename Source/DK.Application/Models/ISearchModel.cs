namespace DK.Application.Models
{
    public interface ISearchModel
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        string SearchText { get; set; }
    }

    public class DefaultSearchModel : ISearchModel
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchText { get; set; }
    }
}
