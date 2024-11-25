namespace DotMarker.Domain.Entities;

public class User
{
    public User(ICollection<Content> contents)
    {
        Contents = new List<Content>();
    }
    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public virtual ICollection<Content> Contents { get; set; }
    public void UpdateFullName(string fullName)
    {
        FullName = fullName;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }

    public void CreateUser(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
    }

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