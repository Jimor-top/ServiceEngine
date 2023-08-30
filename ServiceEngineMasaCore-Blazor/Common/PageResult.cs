namespace ServiceEngineMasaCore.Blazor.Common
{
    public class PageResult<TItem>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalRows { get; set; }
        public ICollection<TItem> Rows { get; set; } = new List<TItem>();
    }
}
