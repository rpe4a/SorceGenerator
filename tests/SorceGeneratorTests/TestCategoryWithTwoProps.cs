using SourceGeneratorLibrary;

namespace SorceGeneratorTests;

[EnumGeneration]
internal partial class TestCategoryWithTwoProps
{
    private readonly string _name;

    public static readonly TestCategoryWithTwoProps Fruits = new("Fruits");
    public static readonly TestCategoryWithTwoProps Dairy = new("Dairy");

    private TestCategoryWithTwoProps(string name)
    {
        _name = name;
    }
}