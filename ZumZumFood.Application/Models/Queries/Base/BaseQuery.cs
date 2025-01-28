namespace ZumZumFood.Application.Models.Queries.Base

{
    public class BaseQuery<T>
    {
        public BaseQuery()
        {
            PageSize = 15;
            SortAscending = true;
            Status = -1;
            SelectAll = false;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public bool SortAscending { get; set; }
        public int Status { get; set; }
        public bool SelectAll { get; set; }
    }
}
