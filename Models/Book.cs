namespace library.Models;

public enum BookCategory
{
    Novel, SciFi, Fantasy, History, Biography, Kids, Education, Technology, Poetry, Other
}

public class Book
{
    public string?  CoverPath { get; set; }
    public string   Title     { get; set; } = "";
    public string   Author    { get; set; } = "";
    public string   ISBN      { get; set; } = "";
    public int      Year      { get; set; }
    public BookCategory Category { get; set; } = BookCategory.Novel;
    public int      Stock     { get; set; }
    public string?  Publisher { get; set; }
}