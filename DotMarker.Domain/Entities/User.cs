namespace DotMarker.Domain.Entities;

public class User
{
    public User(ICollection<Content> contents)
    {
        Contents = new List<Content>();
    }
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Content> Contents { get; set; }

    public void AddContent(Content content)
    {
        Contents.Add(content);
    }

    public void RemoveContent(Content content)
    {
        Contents.Remove(content);
    }

    public void RemoveAllContents()
    {
        Contents.Clear();
    }
}