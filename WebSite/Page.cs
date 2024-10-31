namespace WebSite;

public class Page
{
    public string Title { get; set; }
    public string Type { get; set; }
    public Characteristics Chars { get; set; }
    public bool Authorize { get; set; }
}

public class PageComparer : IComparer<Page>
{
    public int Compare(Page? x, Page? y)
    {
        if (x is null && y is null)
            throw new Exception("Invalid data values");
        return String.Compare(x!.Title, y!.Title, StringComparison.Ordinal);
    }
}