using SourceGeneratorLibrary;

namespace SomeProject;

[EnumGeneration]
public class ProductCategory
{
    public static readonly ProductCategory Fruits = new("Fruits");
    public static readonly ProductCategory Dairy = new("Dairy");
    public static readonly ProductCategory Milk = new("Milk");
    public static readonly ProductCategory Vegetables = new("Vegetables");

    public string Name { get; }

    private ProductCategory(string name)
    {
        Name = name;
    }
}