# NekoSpace.Build.Resources

Embed lightweight JSON resources into your .NET application, instead of traditional .resx files.

## Features

- Parse and embed JSON resource into assembly using MSBuild Task, results the same functionality as ResX.
- Cleaner, more human-readable content compared to ResX, even outside IDEs. (Without IDEs the .resx file is totally a mess)
- Easy to use. Just a few lines to include all JSON resources, so you won't mess up your project file.
- Support nested JSON structures.

## Getting Started

First install this package using package manager or using dotnet CLI:

```
dotnet add package NekoSpace.Build.Resources.Json
```

Then, add a single line to your csproj file:

```xml
<ItemGroup>
    <!-- Embed our JSON resources. -->
    <JsonResource Include="**\*.json" Exclude="bin\**\*; obj\**\*" />
</ItemGroup>
```

Create a simple JSON file, for example called `ExampleResource.json`:

```json
{
  "ExampleKey": "English Content",
  "Group": {
    "ExampleSubKey": "Another English content"
  }
}
```

Here is a example for using the resources:

```csharp
// Get the assembly which embedded our resources.
var assembly = typeof(Program).Assembly;

// Instantiate the ResourceManager to manage our resources.
var manager = new ResourceManager("UsingJsonResources.ExampleResource", assembly);

// Resource defined in ExampleResource.json, and get parsed, embedded automatically.
Console.WriteLine(manager.GetString("ExampleKey"));

// Access nested resource.
Console.WriteLine(manager.GetString("Group:ExampleSubKey"));
```

Same as ResX, our resources can be easily localized!

Create another JSON file called `ExampleResource.zh-Hans.json`:

```json
{
  "ExampleKey": "中文内容"
}
```

Then get the resource string by passing a specified culture:

```csharp
// Override the culture.
Console.WriteLine(manager.GetString("ExampleKey", new CultureInfo("zh-Hans")));
```

You can find more examples [in the examples folder](./examples).

## License

This project is under [MIT license](./LICENSE).
