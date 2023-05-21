using System.Globalization;
using System.Resources;

namespace UsingJsonResources;

public static class Program
{
    public static void Main()
    {
        // We set the CurrentUICulture to English for testing purpose.
        CultureInfo.CurrentUICulture = new CultureInfo("en-US");

        // Get the assembly which embedded our resources.
        var assembly = typeof(Program).Assembly;

        // Instantiate the ResourceManager to manage our resources.
        var manager = new ResourceManager("UsingJsonResources.ExampleResource", assembly);

        // Resource defined in ExampleResource.json, and get parsed, embedded automatically.
        Console.WriteLine(manager.GetString("ExampleKey"));

        // Override the culture.
        Console.WriteLine(manager.GetString("ExampleKey", new CultureInfo("zh-Hans")));

        // Access nested resource.
        Console.WriteLine(manager.GetString("Group:ExampleSubKey"));

        // Alternatively, we can instantiate a ResourceManager using a type.
        // The manager will use the type's full name to find the resource.
        var another = new ResourceManager(typeof(ExampleType));
        Console.WriteLine(another.GetString("TypeResourceKey"));
    }

    class ExampleType
    {
    }
}