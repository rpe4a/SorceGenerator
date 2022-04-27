namespace SomeProject;

partial class Program
{
    static void Main(string[] args)
    {
        foreach (var item in ProductCategory.Items)
        {
            Console.WriteLine(item.Name);
        }
    }

    static partial void HelloFrom(string name);
}