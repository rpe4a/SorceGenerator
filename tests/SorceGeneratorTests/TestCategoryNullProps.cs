using SourceGeneratorLibrary;

namespace SorceGeneratorTests;

[EnumGeneration]
internal partial class TestCategoryNullProps
{
    private readonly string _name;

    private TestCategoryNullProps(string name)
    {
        _name = name;
    }
}