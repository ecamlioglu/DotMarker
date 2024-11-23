namespace DotMarker.Domain.Entities;

public class Content
{
    public Content(ICollection<Variant> variants)
    {
        Variants = new List<Variant>();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public string Language { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<Variant> Variants { get; set; }

    public void AddVariant(Variant variant)
    {
        Variants.Add(variant);
    }

    public void RemoveVariant(int variantId)
    {
        var variant = Variants.FirstOrDefault(x => x.Id == variantId);
        if (variant != null)
        {
            Variants.Remove(variant);
        }
    }

    public void RemoveAllVariants()
    {
        Variants.Clear();
    }
}